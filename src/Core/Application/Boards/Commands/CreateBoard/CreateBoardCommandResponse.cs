using Scrumboard.Application.Common.Models;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Boards.Commands.CreateBoard;

public class CreateBoardCommandResponse : BaseResponse
{
    public CreateBoardCommandResponse() : base() { }

    public BoardDto Board { get; set; }
}