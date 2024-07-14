#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

public sealed class TeamEdition
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IReadOnlyCollection<string> MemberIds { get; set; }
}
