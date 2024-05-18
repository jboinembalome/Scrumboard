using Scrumboard.Application.Common.Models;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Cards.Comments.Commands.CreateComment;

public class CreateCommentCommandResponse : BaseResponse
{
    public CreateCommentCommandResponse() : base() { }

    public CommentDto Comment { get; set; }
}