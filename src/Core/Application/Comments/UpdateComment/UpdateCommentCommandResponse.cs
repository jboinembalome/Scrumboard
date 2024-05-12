using Scrumboard.Application.Dto;
using Scrumboard.Application.Responses;

namespace Scrumboard.Application.Comments.UpdateComment;

public class UpdateCommentCommandResponse : BaseResponse
{
    public UpdateCommentCommandResponse() : base() { }
       
    public CommentDto Comment { get; set; }
}