using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Commands.DeleteBoard;

public class DeleteBoardHandler : IRequestHandler<DeleteBoardCommand>
{
    private readonly IAsyncRepository<Board, int> _boardRepository;
    private readonly IMapper _mapper;

    public DeleteBoardHandler(IMapper mapper, IAsyncRepository<Board, int> boardRepository)
    {
        _mapper = mapper;
        _boardRepository = boardRepository;
    }

    public async Task<Unit> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        var boardToDelete = await _boardRepository.GetByIdAsync(request.BoardId, cancellationToken);

        if (boardToDelete == null)
            throw new NotFoundException(nameof(Board), request.BoardId);

        await _boardRepository.DeleteAsync(boardToDelete, cancellationToken);

        return Unit.Value;
    }
}