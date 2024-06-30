using Scrumboard.Infrastructure.Abstractions.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Persistence.Cards.Checklists;

public sealed class ChecklistItemDao : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsChecked { get; set; }
    public int ChecklistId { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
