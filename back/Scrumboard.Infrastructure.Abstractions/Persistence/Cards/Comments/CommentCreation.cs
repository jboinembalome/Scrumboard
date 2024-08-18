using Scrumboard.Domain.Cards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

public sealed class CommentCreation
{
    public string Message { get; set; } = string.Empty;
    public CardId CardId { get; set; }
}
