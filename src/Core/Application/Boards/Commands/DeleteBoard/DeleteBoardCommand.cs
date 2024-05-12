using MediatR;

namespace Scrumboard.Application.Boards.Commands.DeleteBoard;

public class DeleteBoardCommand : IRequest
{
    public int BoardId { get; set; }
}