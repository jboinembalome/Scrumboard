using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Application.Adherents.Queries.GetAdherents;

internal sealed class GetAdherentsQueryHandler(
    IMapper mapper,
    IIdentityService identityService)
    : IRequestHandler<GetAdherentsQuery, IEnumerable<AdherentDto>>
{
    public async Task<IEnumerable<AdherentDto>> Handle(
        GetAdherentsQuery request, 
        CancellationToken cancellationToken)
    {
        
        var users = await identityService.GetListAllAsync(cancellationToken);

        var adherentDtos = mapper.Map<IEnumerable<AdherentDto>>(users);

        return mapper.Map(users, adherentDtos);
    }
}
