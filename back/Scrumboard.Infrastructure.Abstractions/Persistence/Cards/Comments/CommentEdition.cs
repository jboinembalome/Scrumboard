#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Comments;

public sealed class CommentEdition
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public int CardId { get; set; }
}
