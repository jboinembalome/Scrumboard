using MediatR;

namespace Scrumboard.Application.Boards.Commands.CreateBoard;

public sealed class CreateBoardCommand : IRequest<CreateBoardCommandResponse>
{
    public string UserId { get; set; } = string.Empty;
}
