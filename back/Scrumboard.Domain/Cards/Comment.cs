using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards;

public sealed class Comment : IAuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Message { get; set; }
    public int CardId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
