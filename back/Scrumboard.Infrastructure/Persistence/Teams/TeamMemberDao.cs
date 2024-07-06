#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Persistence.Teams;

public sealed class TeamMemberDao
{
    public int TeamId { get; set; }
    public string MemberId { get; set; }
}
