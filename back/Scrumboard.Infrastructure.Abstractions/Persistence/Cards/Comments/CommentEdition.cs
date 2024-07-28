using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Comments;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

public sealed class CommentEdition
{
    public CommentId Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public CardId CardId { get; set; }
}
