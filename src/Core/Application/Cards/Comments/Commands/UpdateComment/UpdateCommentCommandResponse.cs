using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Cards.Comments.Commands.UpdateComment;

public sealed class UpdateCommentCommandResponse : BaseResponse
{
    public CommentDto Comment { get; set; }
}
