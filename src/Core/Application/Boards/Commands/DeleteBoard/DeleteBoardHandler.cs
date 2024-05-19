﻿using AutoMapper;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Commands.DeleteBoard;

internal sealed class DeleteBoardHandler : IRequestHandler<DeleteBoardCommand>
{
    private readonly IAsyncRepository<Board, int> _boardRepository;
    private readonly IMapper _mapper;

    public DeleteBoardHandler(
        IMapper mapper, 
        IAsyncRepository<Board, int> boardRepository)
    {
        _mapper = mapper;
        _boardRepository = boardRepository;
    }

    public async Task Handle(
        DeleteBoardCommand request, 
        CancellationToken cancellationToken)
    {
        var boardToDelete = await _boardRepository.GetByIdAsync(request.BoardId, cancellationToken);

        if (boardToDelete is null)
            throw new NotFoundException(nameof(Board), request.BoardId);

        await _boardRepository.DeleteAsync(boardToDelete, cancellationToken);
    }
}
