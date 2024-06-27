using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Cards.Activities.Specifications;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Activities.Queries.GetActivitiesByCardId;

internal sealed class GetActivitiesByCardIdQueryHandler(
    IMapper mapper,
    IAsyncRepository<Card, int> cardRepository,
    IIdentityService identityService)
    : IRequestHandler<GetActivitiesByCardIdQuery, IEnumerable<ActivityDto>>
{
    public async Task<IEnumerable<ActivityDto>> Handle(
        GetActivitiesByCardIdQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new AllActivitiesInCardSpec(request.CardId);
        var card = await cardRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (card is null)
        {
            return [];
        }
        
        var activities = card.Activities;
        
        var activityDtos = mapper.Map<IEnumerable<ActivityDto>>(activities).ToList();

        if (activities.Count == 0)
        {
            return activityDtos;
        }
        
        var users = await identityService
            .GetListAsync(activities
                .Select(a => a.CreatedBy), cancellationToken);
        
        var adherentDtos = activityDtos.Select(c => c.Adherent).ToList();

        MapUsers(users, adherentDtos);

        return activityDtos;
    }

    private void MapUsers(IReadOnlyList<IUser> users, IEnumerable<AdherentDto> adherents)
    {
        foreach (var adherent in adherents)
        {
            var user = users.FirstOrDefault(u => u.Id == adherent.Id);
            if (user is null)
                continue;

            mapper.Map(user, adherent);
        }
    }
}
