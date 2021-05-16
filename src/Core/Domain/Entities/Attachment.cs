using Scrumboard.Domain.Common;
using Scrumboard.Domain.Enums;
using Scrumboard.Domain.Interfaces;
using System;

namespace Scrumboard.Domain.Entities
{
    public class Attachment : AuditableEntity, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public Card Card { get; set; }
    }
}
