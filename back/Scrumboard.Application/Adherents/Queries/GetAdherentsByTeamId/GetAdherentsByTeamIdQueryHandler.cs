using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Adherents.Specifications;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Adherents.Queries.GetAdherentsByTeamId;

internal sealed class GetAdherentsByTeamIdQueryHandler(
    IMapper mapper,
    IAsyncRepository<Team, int> teamRepository,
    IIdentityService identityService)
    : IRequestHandler<GetAdherentsByTeamIdQuery, IEnumerable<AdherentDto>>
{
    public async Task<IEnumerable<AdherentDto>> Handle(
        GetAdherentsByTeamIdQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new AllAdherentsInTeamSpec(request.TeamId);
        
        var team = await teamRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (team is null)
        {
            return [];
        }
        
        var adherents = team.Adherents.ToHashSet();
        
        var users = await identityService.GetListAsync(adherents, cancellationToken);

        var adherentDtos = mapper.Map<IEnumerable<AdherentDto>>(adherents);

        return mapper.Map(users, adherentDtos);
    }
}
