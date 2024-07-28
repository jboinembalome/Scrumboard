using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Teams;

public sealed class Team
{
    public TeamId Id { get; set; }
    public string Name { get; set; }
    public ICollection<UserId> MemberIds { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserId? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
