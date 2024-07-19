namespace Scrumboard.Web.Api.Cards.Comments;

public sealed class CommentCreationModel
{
    public string Message { get; set; } = string.Empty;
    public int CardId { get; set; }
}
