using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards.Comments;

public sealed class Comment
{
    public CommentId Id { get; set; }
    public string Message { get; set; }
    public CardId CardId { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserId? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
