using AutoMapper;
using MediatR;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Boards.Specifications;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Boards.Queries.GetBoardDetail
{
    public class GetBoardDetailQueryHandler : IRequestHandler<GetBoardDetailQuery, BoardDetailDto>
    {
        private readonly IAsyncRepository<Board, Guid> _boardRepository;
        private readonly IMapper _mapper;

        public GetBoardDetailQueryHandler(IMapper mapper, IAsyncRepository<Board, Guid> boardRepository)
        {
            _mapper = mapper;
            _boardRepository = boardRepository;
        }

        public async Task<BoardDetailDto> Handle(GetBoardDetailQuery request, CancellationToken cancellationToken)
        {
            var specification = new BoardWithAllSpec(request.BoardId);
            var board = await _boardRepository.FirstOrDefaultAsync(specification, cancellationToken);

            if (board == null)
                throw new NotFoundException(nameof(Board), request.BoardId);

            return _mapper.Map<BoardDetailDto>(board);
        }
    }
}
