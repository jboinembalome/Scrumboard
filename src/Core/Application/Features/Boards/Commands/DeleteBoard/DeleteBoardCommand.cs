using MediatR;
using System;

namespace Scrumboard.Application.Features.Boards.Commands.DeleteBoard
{
    public class DeleteBoardCommand : IRequest
    {
        public Guid BoardId { get; set; }
    }
}
