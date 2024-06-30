namespace Scrumboard.Web.Api.Cards.Comments;

public sealed class CommentEditionModel
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
}
