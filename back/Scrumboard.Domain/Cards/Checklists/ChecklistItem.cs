using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards.Checklists;

public sealed class ChecklistItem : IAuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsChecked { get; set; }
    public int ChecklistId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
