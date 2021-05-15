using Scrumboard.Domain.Common;
using Scrumboard.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class Checklist : AuditableEntity, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Card Card { get; set; }
        public ICollection<ChecklistItem> ChecklistItems { get; set; }
    }
}
