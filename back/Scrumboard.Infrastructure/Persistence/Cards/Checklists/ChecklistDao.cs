using Scrumboard.Infrastructure.Abstractions.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Persistence.Cards.Checklists;

public sealed class ChecklistDao : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CardId { get; set; }
    public ICollection<ChecklistItemDao> ChecklistItems { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}
