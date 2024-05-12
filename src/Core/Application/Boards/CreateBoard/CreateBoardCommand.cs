using MediatR;

namespace Scrumboard.Application.Boards.CreateBoard;

public class CreateBoardCommand : IRequest<CreateBoardCommandResponse>
{
    public string UserId { get; set; }
}