using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Boards.Dtos;

namespace Scrumboard.Application.Boards.Queries.GetBoardsByUserId;

public class GetBoardsByUserIdQuery : IRequest<IEnumerable<BoardDto>>
{
    public string UserId { get; set; }
}