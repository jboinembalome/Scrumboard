using Scrumboard.Web.Api.Users;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Cards.Comments;

public sealed class CommentDto
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
    public UserDto User { get; set; }
    public int CardId { get; set; }
}
