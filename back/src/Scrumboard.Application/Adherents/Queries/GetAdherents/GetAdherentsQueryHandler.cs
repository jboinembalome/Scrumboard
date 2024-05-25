using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Domain.Adherents;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Adherents.Queries.GetAdherents;

internal sealed class GetAdherentsQueryHandler(
    IMapper mapper,
    IAsyncRepository<Adherent, int> adherentRepository,
    IIdentityService identityService)
    : IRequestHandler<GetAdherentsQuery, IEnumerable<AdherentDto>>
{
    public async Task<IEnumerable<AdherentDto>> Handle(
        GetAdherentsQuery request, 
        CancellationToken cancellationToken)
    {
        var adherents = await adherentRepository.ListAllAsync(cancellationToken);

        var userIds = adherents.Select(a => a.IdentityId);
        var users = await identityService.GetListAsync(userIds, cancellationToken);

        var adherentDtos = mapper.Map<IEnumerable<AdherentDto>>(adherents);

        return mapper.Map(users, adherentDtos);
    }
}
