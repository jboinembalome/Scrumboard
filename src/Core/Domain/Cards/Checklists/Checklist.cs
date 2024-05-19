using System;
using System.Collections.Generic;
using Scrumboard.Domain.Common;

namespace Scrumboard.Domain.Cards.Checklists;

public sealed class Checklist : IAuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Card Card { get; set; }
    public ICollection<ChecklistItem> ChecklistItems { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}