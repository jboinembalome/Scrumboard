using Scrumboard.Domain.Common;
using System;

namespace Scrumboard.Domain.Entities
{
    public class Checklist : AuditableEntity
    {
        public Guid ChecklistId { get; set; }
        public string Name { get; set; }
        public Card Card { get; set; }
    }
}
