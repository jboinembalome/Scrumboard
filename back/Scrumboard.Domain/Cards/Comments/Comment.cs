using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards.Comments;

public sealed class Comment : AuditableEntityBase<CommentId>
{
    
    public string Message { get; set; }
    public CardId CardId { get; set; }
}
