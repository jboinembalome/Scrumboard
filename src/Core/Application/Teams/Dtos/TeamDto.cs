using System.Collections.Generic;
using Scrumboard.Application.Adherents.Dtos;

namespace Scrumboard.Application.Teams.Dtos;

public class TeamDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<AdherentDto> Adherents { get; set; }
}