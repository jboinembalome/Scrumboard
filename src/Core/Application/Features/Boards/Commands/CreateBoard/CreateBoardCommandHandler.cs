﻿using AutoMapper;
using MediatR;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Boards.Commands.CreateBoard
{
    public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, CreateBoardCommandResponse>
    {
        private readonly IAsyncRepository<Board, Guid> _boardRepository;
        private readonly IMapper _mapper;

        public CreateBoardCommandHandler(IMapper mapper, IAsyncRepository<Board, Guid> boardRepository)
        {
            _mapper = mapper;
            _boardRepository = boardRepository;
        }

        public async Task<CreateBoardCommandResponse> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            var createBoardCommandResponse = new CreateBoardCommandResponse();

            var board = _mapper.Map<Board>(request);
            board = await _boardRepository.AddAsync(board, cancellationToken);

            createBoardCommandResponse.Board = _mapper.Map<BoardDto>(board);

            return createBoardCommandResponse;
        }
    }
}
