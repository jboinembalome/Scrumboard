using MediatR;
using Scrumboard.Application.Users.Dtos;

namespace Scrumboard.Application.Users.Queries.GetUsersByTeamId;

public sealed class GetUsersByTeamIdQuery : IRequest<IEnumerable<UserDto>>
{
    public int TeamId { get; set; }
}
