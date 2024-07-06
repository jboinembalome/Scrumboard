using MediatR;
using Scrumboard.Application.Users.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Teams.Commands.UpdateTeam;

public sealed class UpdateTeamCommand : IRequest<UpdateTeamCommandResponse>
{
    public int Id { get; set; }
    public IEnumerable<UserDto> Members { get; set; }
}
