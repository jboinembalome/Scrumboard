using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Adherents.Specifications;
using Scrumboard.Domain.Adherents;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Adherents.Queries.GetAdherentsByTeamId;

internal sealed class GetAdherentsByTeamIdQueryHandler(
    IMapper mapper,
    IAsyncRepository<Adherent, int> adherentRepository,
    IIdentityService identityService)
    : IRequestHandler<GetAdherentsByTeamIdQuery, IEnumerable<AdherentDto>>
{
    public async Task<IEnumerable<AdherentDto>> Handle(
        GetAdherentsByTeamIdQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new AllAdherentsInTeamSpec(request.TeamId);
        
        var adherents = await adherentRepository.ListAsync(specification, cancellationToken);

        var userIds = adherents.Select(a => a.IdentityId);
        var users = await identityService.GetListAsync(userIds, cancellationToken);

        var adherentDtos = mapper.Map<IEnumerable<AdherentDto>>(adherents);

        return mapper.Map(users, adherentDtos);
    }
}
