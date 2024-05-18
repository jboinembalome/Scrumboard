using Scrumboard.Application.Common.Models;
using Scrumboard.Application.Teams.Dtos;

namespace Scrumboard.Application.Teams.Commands.UpdateTeam;

public class UpdateTeamCommandResponse : BaseResponse
{
    public UpdateTeamCommandResponse() : base() { }
       
    public TeamDto Team { get; set; }
}