using MediatR;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Boards.Queries.GetBoardDetail;

public class GetBoardDetailQuery : IRequest<BoardDetailDto>
{
    public int BoardId { get; set; }
}