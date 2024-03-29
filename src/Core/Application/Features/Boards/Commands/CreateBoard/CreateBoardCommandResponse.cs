﻿using Scrumboard.Application.Dto;
using Scrumboard.Application.Responses;

namespace Scrumboard.Application.Features.Boards.Commands.CreateBoard
{
    public class CreateBoardCommandResponse : BaseResponse
    {
        public CreateBoardCommandResponse() : base() { }

        public BoardDto Board { get; set; }
    }
}
