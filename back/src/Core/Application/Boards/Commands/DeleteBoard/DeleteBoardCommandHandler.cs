using AutoMapper;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Commands.DeleteBoard;

internal sealed class DeleteBoardCommandHandler(
    IMapper mapper,
    IAsyncRepository<Board, int> boardRepository)
    : IRequestHandler<DeleteBoardCommand>
{
    private readonly IMapper _mapper = mapper;

    public async Task Handle(
        DeleteBoardCommand request, 
        CancellationToken cancellationToken)
    {
        var boardToDelete = await boardRepository.GetByIdAsync(request.BoardId, cancellationToken);

        if (boardToDelete is null)
            throw new NotFoundException(nameof(Board), request.BoardId);

        await boardRepository.DeleteAsync(boardToDelete, cancellationToken);
    }
}
