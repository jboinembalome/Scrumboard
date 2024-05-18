using MediatR;

namespace Scrumboard.Application.Boards.Commands.CreateBoard;

public class CreateBoardCommand : IRequest<CreateBoardCommandResponse>
{
    public string UserId { get; set; }
}