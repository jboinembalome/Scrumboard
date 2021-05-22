using MediatR;

namespace Scrumboard.Application.Features.Boards.Commands.DeleteBoard
{
    public class DeleteBoardCommand : IRequest
    {
        public int BoardId { get; set; }
    }
}
