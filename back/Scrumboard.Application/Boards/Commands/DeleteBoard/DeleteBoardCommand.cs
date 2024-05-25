using MediatR;

namespace Scrumboard.Application.Boards.Commands.DeleteBoard;

public sealed class DeleteBoardCommand : IRequest
{
    public int BoardId { get; set; }
}