﻿using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards.Checklists;

public sealed class Checklist : IAuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CardId { get; set; }
    public ICollection<ChecklistItem> ChecklistItems { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
