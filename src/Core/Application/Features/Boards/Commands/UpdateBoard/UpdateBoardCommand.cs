using MediatR;
using System;

namespace Scrumboard.Application.Features.Boards.Commands.UpdateBoard
{
    public class UpdateBoardCommand : IRequest
    {
        public Guid BoardId { get; set; }
        public string Name { get; set; }
    }
}
