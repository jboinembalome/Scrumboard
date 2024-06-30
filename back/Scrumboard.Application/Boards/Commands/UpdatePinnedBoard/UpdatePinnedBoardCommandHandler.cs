using AutoMapper;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.Boards.Commands.UpdatePinnedBoard;

internal sealed class UpdatePinnedBoardCommandHandler(
    IMapper mapper,
    IBoardsRepository boardsRepository)
    : IRequestHandler<UpdatePinnedBoardCommand>
{
    public async Task Handle(
        UpdatePinnedBoardCommand request, 
        CancellationToken cancellationToken)
    {
        var boardToUpdate = await boardsRepository.TryGetByIdAsync(request.BoardId, cancellationToken);

        if (boardToUpdate is null)
            throw new NotFoundException(nameof(Board), request.BoardId);

        mapper.Map(request, boardToUpdate);

        await boardsRepository.UpdateAsync(boardToUpdate, cancellationToken);
    }
}
