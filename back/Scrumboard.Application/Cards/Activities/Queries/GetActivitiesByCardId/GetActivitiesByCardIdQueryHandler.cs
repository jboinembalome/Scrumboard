using AutoMapper;
using MediatR;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Users.Dtos;
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
        
        var userDtos = activityDtos.Select(c => c.User).ToList();

        MapUsers(users, userDtos);

        return activityDtos;
    }

    private void MapUsers(IReadOnlyList<IUser> users, IEnumerable<UserDto> userDtos)
    {
        foreach (var userDto in userDtos)
        {
            var user = users.FirstOrDefault(u => u.Id == userDto.Id);

            if (user is null)
            {
                continue;
            }

            mapper.Map(user, userDto);
        }
    }
}
