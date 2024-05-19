using Scrumboard.Application.Adherents.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Teams.Dtos;

public sealed class TeamDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<AdherentDto> Adherents { get; set; }
}
