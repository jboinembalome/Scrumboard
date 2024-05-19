using System;
using Scrumboard.Domain.Common;

namespace Scrumboard.Domain.Cards.Attachments;

public sealed class Attachment : IAuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public AttachmentType AttachmentType { get; set; }
    public Card Card { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}