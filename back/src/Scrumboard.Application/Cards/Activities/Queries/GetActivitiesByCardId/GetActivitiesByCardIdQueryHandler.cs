using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Cards.Activities.Specifications;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Cards.Activities.Queries.GetActivitiesByCardId;

internal sealed class GetActivitiesByCardIdQueryHandler(
    IMapper mapper,
    IAsyncRepository<Activity, int> activityRepository,
    IIdentityService identityService)
    : IRequestHandler<GetActivitiesByCardIdQuery, IEnumerable<ActivityDto>>
{
    public async Task<IEnumerable<ActivityDto>> Handle(
        GetActivitiesByCardIdQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new AllActivitiesInCardSpec(request.CardId);
        var activities = await activityRepository.ListAsync(specification, cancellationToken);
        var activityDtos = mapper.Map<IEnumerable<ActivityDto>>(activities);

        if (!activities.Any()) return activityDtos;
        
        var users = await identityService.GetListAsync(activities.Select(a => a.Adherent.IdentityId), cancellationToken);
        var adherentDtos = activityDtos.Select(c => c.Adherent).ToList();

        MapUsers(users, adherentDtos);

        return activityDtos;
    }

    private void MapUsers(IEnumerable<IUser> users, IEnumerable<AdherentDto> adherents)
    {
        foreach (var adherent in adherents)
        {
            var user = users.FirstOrDefault(u => u.Id == adherent.IdentityId);
            if (user == null)
                continue;

            mapper.Map(user, adherent);
        }
    }
}
