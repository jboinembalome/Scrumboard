using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

public sealed class CommentCreation
{
    public string Message { get; set; } = string.Empty;
    public CardId CardId { get; set; }
}
