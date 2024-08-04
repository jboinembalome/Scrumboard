using Scrumboard.Infrastructure.Abstractions.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Persistence.Teams;

public sealed class TeamDao : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<TeamMemberDao> Members { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}
