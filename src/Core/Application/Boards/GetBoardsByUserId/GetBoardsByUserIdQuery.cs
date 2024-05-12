using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Boards.GetBoardsByUserId;

public class GetBoardsByUserIdQuery : IRequest<IEnumerable<BoardDto>>
{
    public string UserId { get; set; }
}