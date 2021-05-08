using Scrumboard.Domain.Common;
using System;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class Card : AuditableEntity
    {
        public Guid CardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Suscribed { get; set; }
        public DateTime DueDate { get; set; }
        public ListBoard ListBoard { get; set; }
        public ICollection<Label> Labels { get; set; }
        public ICollection<Guid> UserIds { get; set; }
    }
}
