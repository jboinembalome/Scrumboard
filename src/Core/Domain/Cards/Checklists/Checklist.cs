using System.Collections.Generic;
using Scrumboard.Domain.Common;

namespace Scrumboard.Domain.Cards.Checklists;

public class Checklist : AuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Card Card { get; set; }
    public ICollection<ChecklistItem> ChecklistItems { get; set; }
}