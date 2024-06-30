using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.Boards.Commands.DeleteBoard;

internal sealed class DeleteBoardCommandHandler(
    IBoardsRepository boardsRepository)
    : IRequestHandler<DeleteBoardCommand>
{
    public async Task Handle(
        DeleteBoardCommand request, 
        CancellationToken cancellationToken)
    {
        var boardToDelete = await boardsRepository.TryGetByIdAsync(request.BoardId, cancellationToken);

        if (boardToDelete is null)
            throw new NotFoundException(nameof(Board), request.BoardId);

        await boardsRepository.DeleteAsync(boardToDelete.Id, cancellationToken);
    }
}
