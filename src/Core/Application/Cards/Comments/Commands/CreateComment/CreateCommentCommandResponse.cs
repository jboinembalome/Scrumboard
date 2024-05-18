using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Models;

namespace Scrumboard.Application.Cards.Comments.Commands.CreateComment;

public class CreateCommentCommandResponse : BaseResponse
{
    public CreateCommentCommandResponse() : base() { }

    public CommentDto Comment { get; set; }
}