﻿using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.Common.Models;

namespace Scrumboard.Application.Boards.Commands.CreateBoard;

public class CreateBoardCommandResponse : BaseResponse
{
    public CreateBoardCommandResponse() : base() { }

    public BoardDto Board { get; set; }
}