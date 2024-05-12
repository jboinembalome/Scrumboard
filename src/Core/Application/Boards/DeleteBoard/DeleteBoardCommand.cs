using MediatR;

namespace Scrumboard.Application.Boards.DeleteBoard;

public class DeleteBoardCommand : IRequest
{
    public int BoardId { get; set; }
}