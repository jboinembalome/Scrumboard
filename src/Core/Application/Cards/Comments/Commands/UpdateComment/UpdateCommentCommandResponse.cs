using Scrumboard.Application.Common.Models;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Cards.Comments.Commands.UpdateComment;

public class UpdateCommentCommandResponse : BaseResponse
{
    public UpdateCommentCommandResponse() : base() { }
       
    public CommentDto Comment { get; set; }
}