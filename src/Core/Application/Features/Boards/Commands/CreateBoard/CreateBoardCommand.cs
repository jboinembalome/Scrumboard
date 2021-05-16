using MediatR;
using System;

namespace Scrumboard.Application.Features.Boards.Commands.CreateBoard
{
    public class CreateBoardCommand : IRequest<CreateBoardCommandResponse>
    {
        public Guid UserId { get; set; }
    }
}
