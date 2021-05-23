using MediatR;
using Scrumboard.Application.Dto;
using System;

namespace Scrumboard.Application.Features.Boards.Queries.GetBoardDetail
{
    public class GetBoardDetailQuery : IRequest<BoardDetailDto>
    {
        public int BoardId { get; set; }
    }
}
