using MediatR;

namespace Scrumboard.Application.Cards.Comments.Commands.DeleteComment;

public class DeleteCommentCommand : IRequest
{
    public int CommentId { get; set; }
}