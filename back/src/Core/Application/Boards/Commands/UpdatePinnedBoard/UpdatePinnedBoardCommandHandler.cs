using AutoMapper;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Commands.UpdatePinnedBoard;

internal sealed class UpdatePinnedBoardCommandHandler(
    IMapper mapper,
    IAsyncRepository<Board, int> boardRepository)
    : IRequestHandler<UpdatePinnedBoardCommand>
{
    public async Task Handle(
        UpdatePinnedBoardCommand request, 
        CancellationToken cancellationToken)
    {
        var boardToUpdate = await boardRepository.GetByIdAsync(request.BoardId, cancellationToken);

        if (boardToUpdate == null)
            throw new NotFoundException(nameof(Board), request.BoardId);

        mapper.Map(request, boardToUpdate, typeof(UpdatePinnedBoardCommand), typeof(Board));

        await boardRepository.UpdateAsync(boardToUpdate, cancellationToken);
    }
}
