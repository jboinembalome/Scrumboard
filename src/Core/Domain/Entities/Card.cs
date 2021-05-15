using Scrumboard.Domain.Common;
using Scrumboard.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class Card : AuditableEntity, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Suscribed { get; set; }
        public DateTime DueDate { get; set; }
        public ListBoard ListBoard { get; set; }
        public ICollection<Label> Labels { get; set; }
        public ICollection<Guid> UserIds { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public ICollection<Checklist> Checklists { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
