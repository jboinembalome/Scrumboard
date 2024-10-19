using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards.Comments;

public sealed class Comment : CreatedAtEntityBase<CommentId>
{
    // TODO: Use private empty constructor for all entities
    private Comment() { }

    public Comment(
        string message, 
        CardId cardId)
    {
        Message = message;
        CardId = cardId;
    }
    
    public string Message { get; private set; }
    public CardId CardId { get; private set; }
    
    public void Update(string message)
    {
        Message = message;
    }
}
