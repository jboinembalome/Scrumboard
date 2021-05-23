using MediatR;

namespace Scrumboard.Application.Features.Boards.Commands.UpdateBoard
{
    public class UpdateBoardCommand : IRequest
    {
        public int BoardId { get; set; }
        public string Name { get; set; }
    }
}
