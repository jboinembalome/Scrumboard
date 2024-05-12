using Scrumboard.Application.Dto;
using Scrumboard.Application.Responses;

namespace Scrumboard.Application.Comments.CreateComment;

public class CreateCommentCommandResponse : BaseResponse
{
    public CreateCommentCommandResponse() : base() { }

    public CommentDto Comment { get; set; }
}