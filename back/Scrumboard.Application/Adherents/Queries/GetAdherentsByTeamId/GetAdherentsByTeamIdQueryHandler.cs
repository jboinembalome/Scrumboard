using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Application.Adherents.Queries.GetAdherentsByTeamId;

internal sealed class GetAdherentsByTeamIdQueryHandler(
    IMapper mapper,
    ITeamsQueryRepository teamsQueryRepository,
    IIdentityService identityService)
    : IRequestHandler<GetAdherentsByTeamIdQuery, IEnumerable<AdherentDto>>
{
    public async Task<IEnumerable<AdherentDto>> Handle(
        GetAdherentsByTeamIdQuery request, 
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

        var adherentDtos = mapper.Map<IEnumerable<AdherentDto>>(members);

        return mapper.Map(users, adherentDtos);
    }
}
