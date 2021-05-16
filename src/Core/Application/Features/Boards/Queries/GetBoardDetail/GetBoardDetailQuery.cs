using MediatR;
using Scrumboard.Application.Dto;
using System;

namespace Scrumboard.Application.Features.Boards.Queries.GetBoardDetail
{
    public class GetBoardDetailQuery : IRequest<BoardDetailDto>
    {
        public Guid BoardId { get; set; }
    }
}
