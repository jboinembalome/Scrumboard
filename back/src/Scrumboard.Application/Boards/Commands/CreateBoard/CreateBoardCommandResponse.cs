using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.Common.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Boards.Commands.CreateBoard;

public sealed class CreateBoardCommandResponse : BaseResponse
{
    public BoardDto Board { get; set; }
}
