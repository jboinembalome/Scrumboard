using Scrumboard.Domain.Common;

namespace Scrumboard.Domain.Cards.Checklists;

public sealed class ChecklistItem : AuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsChecked { get; set; }
    public Checklist Checklist { get; set; }
}