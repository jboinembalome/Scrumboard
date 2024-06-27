using MediatR;

namespace Scrumboard.Application.Boards.Commands.CreateBoard;

public sealed class CreateBoardCommand : IRequest<CreateBoardCommandResponse>
{
    public Guid CreatorId { get; set; }
}
