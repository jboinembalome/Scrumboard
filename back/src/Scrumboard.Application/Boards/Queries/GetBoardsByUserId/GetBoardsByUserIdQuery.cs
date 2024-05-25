using MediatR;
using Scrumboard.Application.Boards.Dtos;

namespace Scrumboard.Application.Boards.Queries.GetBoardsByUserId;

public sealed class GetBoardsByUserIdQuery : IRequest<IEnumerable<BoardDto>>
{
    public string UserId { get; set; } = string.Empty;
}
