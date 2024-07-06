using AutoMapper;
using MediatR;
using Scrumboard.Application.Users.Dtos;
using Scrumboard.Infrastructure.Abstractions.Identity;

namespace Scrumboard.Application.Users.Queries.GetUsers;

internal sealed class GetUsersQueryHandler(
    IMapper mapper,
    IIdentityService identityService)
    : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(
        GetUsersQuery request, 
        CancellationToken cancellationToken)
    {
        
        var users = await identityService.GetListAllAsync(cancellationToken);

        var userDtos = mapper.Map<IEnumerable<UserDto>>(users);

        return mapper.Map(users, userDtos);
    }
}
