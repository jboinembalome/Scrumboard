using Scrumboard.Application.Common.Models;
using Scrumboard.Application.Teams.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Teams.Commands.UpdateTeam;

public sealed class UpdateTeamCommandResponse : BaseResponse
{
    public TeamDto Team { get; set; }
}
