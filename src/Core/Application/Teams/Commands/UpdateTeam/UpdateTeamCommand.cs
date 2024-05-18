using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;

namespace Scrumboard.Application.Teams.Commands.UpdateTeam;

public sealed class UpdateTeamCommand : IRequest<UpdateTeamCommandResponse>
{
    public int Id { get; set; }
    public IEnumerable<AdherentDto> Adherents { get; set; }
}