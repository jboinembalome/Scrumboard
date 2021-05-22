using MediatR;

namespace Scrumboard.Application.Features.Boards.Commands.CreateBoard
{
    public class CreateBoardCommand : IRequest<CreateBoardCommandResponse>
    {
        public string UserId { get; set; }
    }
}
