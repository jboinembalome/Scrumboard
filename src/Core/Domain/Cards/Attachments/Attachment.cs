using Scrumboard.Domain.Common;

namespace Scrumboard.Domain.Cards.Attachments;

public class Attachment : AuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public AttachmentType AttachmentType { get; set; }
    public Card Card { get; set; }
}