using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Commands.UpdatePinnedBoard;

internal sealed class UpdatePinnedBoardCommandHandler : IRequestHandler<UpdatePinnedBoardCommand>
{
    private readonly IAsyncRepository<Board, int> _boardRepository;
    private readonly IMapper _mapper;

    public UpdatePinnedBoardCommandHandler(IMapper mapper, IAsyncRepository<Board, int> boardRepository)
    {
        _mapper = mapper;
        _boardRepository = boardRepository;
    }

    public async Task<Unit> Handle(UpdatePinnedBoardCommand request, CancellationToken cancellationToken)
    {
        var boardToUpdate = await _boardRepository.GetByIdAsync(request.BoardId, cancellationToken);

        if (boardToUpdate == null)
            throw new NotFoundException(nameof(Board), request.BoardId);

        _mapper.Map(request, boardToUpdate, typeof(UpdatePinnedBoardCommand), typeof(Board));

        await _boardRepository.UpdateAsync(boardToUpdate, cancellationToken);

        return Unit.Value;
    }
}