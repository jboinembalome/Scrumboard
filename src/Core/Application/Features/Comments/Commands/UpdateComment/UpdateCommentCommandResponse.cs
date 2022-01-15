using Scrumboard.Application.Dto;
using Scrumboard.Application.Responses;

namespace Scrumboard.Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandResponse : BaseResponse
    {
        public UpdateCommentCommandResponse() : base() { }
       
        public CommentDto Comment { get; set; }
    }
}
