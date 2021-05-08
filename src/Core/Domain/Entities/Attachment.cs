using Scrumboard.Domain.Common;
using Scrumboard.Domain.Enums;
using System;

namespace Scrumboard.Domain.Entities
{
    public class Attachment : AuditableEntity
    {
        public Guid AttachmentId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public Card Card { get; set; }
    }
}
