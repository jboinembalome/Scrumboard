﻿using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Application.Common.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Cards.Comments.Commands.CreateComment;

public sealed class CreateCommentCommandResponse : BaseResponse
{
    public CommentDto Comment { get; set; }
}
