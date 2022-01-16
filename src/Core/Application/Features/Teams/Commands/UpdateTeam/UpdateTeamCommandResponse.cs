using Scrumboard.Application.Dto;
using Scrumboard.Application.Responses;

namespace Scrumboard.Application.Features.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommandResponse : BaseResponse
    {
        public UpdateTeamCommandResponse() : base() { }
       
        public TeamDto Team { get; set; }
    }
}
