using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;

namespace Scrumboard.Application.Cards.Activities.Queries.GetActivitiesByCardId;

internal sealed class GetActivitiesByCardIdQueryHandler(
    IMapper mapper,
    IActivitiesQueryRepository activitiesQueryRepository,
    IIdentityService identityService)
    : IRequestHandler<GetActivitiesByCardIdQuery, IEnumerable<ActivityDto>>
{
    public async Task<IEnumerable<ActivityDto>> Handle(
        GetActivitiesByCardIdQuery request, 
        CancellationToken cancellationToken)
    {
        var activities = await activitiesQueryRepository.GetByCardIdAsync(request.CardId, cancellationToken);

        if (activities.Count == 0)
        {
            return [];
        }
        
        var activityDtos = mapper.Map<IEnumerable<ActivityDto>>(activities).ToList();
        
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
            {
                continue;
            }

            mapper.Map(user, adherent);
        }
    }
}
