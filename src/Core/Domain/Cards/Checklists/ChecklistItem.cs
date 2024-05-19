using System;
using Scrumboard.Domain.Common;

namespace Scrumboard.Domain.Cards.Checklists;

public sealed class ChecklistItem : IAuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsChecked { get; set; }
    public Checklist Checklist { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}