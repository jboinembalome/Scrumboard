using MediatR;
using Scrumboard.Application.Dto;
using System.Collections.Generic;

namespace Scrumboard.Application.Features.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommand : IRequest<UpdateTeamCommandResponse>
    {
        public int Id { get; set; }
        public IEnumerable<AdherentDto> Adherents { get; set; }
    }
}
