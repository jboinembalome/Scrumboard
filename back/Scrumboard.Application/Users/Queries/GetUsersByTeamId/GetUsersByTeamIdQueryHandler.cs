using AutoMapper;
using MediatR;
using Scrumboard.Application.Users.Dtos;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Application.Users.Queries.GetUsersByTeamId;

internal sealed class GetUsersByTeamIdQueryHandler(
    IMapper mapper,
    ITeamsQueryRepository teamsQueryRepository,
    IIdentityService identityService)
    : IRequestHandler<GetUsersByTeamIdQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(
        GetUsersByTeamIdQuery request, 
        CancellationToken cancellationToken)
    {
        var team = await teamsQueryRepository.TryGetByIdAsync(request.TeamId, cancellationToken);

        if (team is null)
        {
            return [];
        }
        
        var members = team.Members
            .ToHashSet();
        
        var users = await identityService.GetListAsync(members, cancellationToken);

        var userDtos = mapper.Map<IEnumerable<UserDto>>(members);

        return mapper.Map(users, userDtos);
    }
}
