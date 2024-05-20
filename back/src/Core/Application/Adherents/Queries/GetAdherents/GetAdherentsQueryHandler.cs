using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Domain.Adherents;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Adherents.Queries.GetAdherents;

internal sealed class GetAdherentsQueryHandler : IRequestHandler<GetAdherentsQuery, IEnumerable<AdherentDto>>
{
    private readonly IAsyncRepository<Adherent, int> _adherentRepository;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetAdherentsQueryHandler(
        IMapper mapper, 
        IAsyncRepository<Adherent, int> adherentRepository, 
        IIdentityService identityService)
    {
        _mapper = mapper;
        _adherentRepository = adherentRepository;
        _identityService = identityService;
    }

    public async Task<IEnumerable<AdherentDto>> Handle(
        GetAdherentsQuery request, 
        CancellationToken cancellationToken)
    {
        var adherents = await _adherentRepository.ListAllAsync(cancellationToken);

        var userIds = adherents.Select(a => a.IdentityId);
        var users = await _identityService.GetListAsync(userIds, cancellationToken);

        var adherentDtos = _mapper.Map<IEnumerable<AdherentDto>>(adherents);

        return _mapper.Map(users, adherentDtos);
    }
}