using MediatR;

namespace Scrumboard.Application.Boards.Commands.UpdatePinnedBoard;

public class UpdatePinnedBoardCommand : IRequest
{
    public int BoardId { get; set; }
    public bool IsPinned { get; set; }
}