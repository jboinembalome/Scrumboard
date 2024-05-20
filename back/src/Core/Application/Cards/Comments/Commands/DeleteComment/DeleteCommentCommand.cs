using MediatR;

namespace Scrumboard.Application.Cards.Comments.Commands.DeleteComment;

public sealed class DeleteCommentCommand : IRequest
{
    public int CommentId { get; set; }
}