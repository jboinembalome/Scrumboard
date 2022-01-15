using MediatR;

namespace Scrumboard.Application.Features.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommand : IRequest
    {
        public int CommentId { get; set; }
    }
}
