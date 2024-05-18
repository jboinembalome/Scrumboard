using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Adherents.Queries.GetAdherents;
using Scrumboard.Domain.Adherents;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Adherents.Queries.GetAdherentsByTeamId;

internal sealed class GetAdherentsByTeamIdQueryHandler : IRequestHandler<GetAdherentsByTeamIdQuery, IEnumerable<AdherentDto>>
{
    private readonly IAsyncRepository<Adherent, int> _adherentRepository;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetAdherentsByTeamIdQueryHandler(
        IMapper mapper, 
        IAsyncRepository<Adherent, int> adherentRepository, 
        IIdentityService identityService)
    {
        _mapper = mapper;
        _adherentRepository = adherentRepository;
        _identityService = identityService;
    }

    public async Task<IEnumerable<AdherentDto>> Handle(
        GetAdherentsByTeamIdQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new AllAdherentsInTeamSpec(request.TeamId);
        
        var adherents = await _adherentRepository.ListAsync(specification, cancellationToken);

        var userIds = adherents.Select(a => a.IdentityId);
        var users = await _identityService.GetListAsync(userIds, cancellationToken);

        var adherentDtos = _mapper.Map<IEnumerable<AdherentDto>>(adherents);

        return _mapper.Map(users, adherentDtos);
    }
}