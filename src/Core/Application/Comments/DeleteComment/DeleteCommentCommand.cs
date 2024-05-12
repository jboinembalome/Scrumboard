using MediatR;

namespace Scrumboard.Application.Comments.DeleteComment;

public class DeleteCommentCommand : IRequest
{
    public int CommentId { get; set; }
}