using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Models;

namespace Scrumboard.Application.Cards.Comments.Commands.UpdateComment;

public sealed class UpdateCommentCommandResponse : BaseResponse
{
    public UpdateCommentCommandResponse() : base() { }
       
    public CommentDto Comment { get; set; }
}