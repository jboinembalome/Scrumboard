namespace Scrumboard.Web.Api.Cards.Comments;

public sealed class CommentEditionDto
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public int CardId { get; set; }
}
