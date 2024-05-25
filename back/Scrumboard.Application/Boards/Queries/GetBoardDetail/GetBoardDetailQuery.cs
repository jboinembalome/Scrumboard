using MediatR;
using Scrumboard.Application.Boards.Dtos;

namespace Scrumboard.Application.Boards.Queries.GetBoardDetail;

public sealed class GetBoardDetailQuery : IRequest<BoardDetailDto>
{
    public int BoardId { get; set; }
}