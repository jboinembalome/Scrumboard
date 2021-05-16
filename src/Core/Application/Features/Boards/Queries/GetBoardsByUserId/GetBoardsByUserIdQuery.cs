using MediatR;
using Scrumboard.Application.Dto;
using System;
using System.Collections.Generic;

namespace Scrumboard.Application.Features.Boards.Queries.GetBoardsByUserId
{
    public class GetBoardsByUserIdQuery : IRequest<IEnumerable<BoardDto>>
    {
        public Guid UserId { get; set; }
    }
}
