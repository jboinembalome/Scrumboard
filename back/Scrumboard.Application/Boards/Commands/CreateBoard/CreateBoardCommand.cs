using MediatR;

namespace Scrumboard.Application.Boards.Commands.CreateBoard;

public sealed class CreateBoardCommand : IRequest<CreateBoardCommandResponse>
{
    public string CreatorId { get; set; } = string.Empty;
}
