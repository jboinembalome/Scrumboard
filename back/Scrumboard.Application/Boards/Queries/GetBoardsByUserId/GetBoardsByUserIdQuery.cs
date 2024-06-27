using MediatR;
using Scrumboard.Application.Boards.Dtos;

namespace Scrumboard.Application.Boards.Queries.GetBoardsByUserId;

public sealed class GetBoardsByUserIdQuery : IRequest<IEnumerable<BoardDto>>
{
    public Guid UserId { get; set; }
}
