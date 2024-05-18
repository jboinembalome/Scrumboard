using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Teams.Commands.UpdateTeam;

public class UpdateTeamCommand : IRequest<UpdateTeamCommandResponse>
{
    public int Id { get; set; }
    public IEnumerable<AdherentDto> Adherents { get; set; }
}