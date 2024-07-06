using MediatR;
using Scrumboard.Application.Users.Dtos;

namespace Scrumboard.Application.Users.Queries.GetUsers;

public sealed class GetUsersQuery : IRequest<IEnumerable<UserDto>>
{
}
