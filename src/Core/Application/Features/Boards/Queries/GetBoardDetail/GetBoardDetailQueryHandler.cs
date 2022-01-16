using AutoMapper;
using MediatR;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Boards.Specifications;
using Scrumboard.Application.Interfaces.Identity;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Boards.Queries.GetBoardDetail
{
    public class GetBoardDetailQueryHandler : IRequestHandler<GetBoardDetailQuery, BoardDetailDto>
    {
        private readonly IAsyncRepository<Board, int> _boardRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetBoardDetailQueryHandler(IMapper mapper, IAsyncRepository<Board, int> boardRepository, IIdentityService identityService)
        {
            _mapper = mapper;
            _boardRepository = boardRepository;
            _identityService = identityService;
        }

        public async Task<BoardDetailDto> Handle(GetBoardDetailQuery request, CancellationToken cancellationToken)
        {
            var specification = new BoardWithAllSpec(request.BoardId);
            var board = await _boardRepository.FirstOrDefaultAsync(specification, cancellationToken);

            if (board == null)
                throw new NotFoundException(nameof(Board), request.BoardId);

            var users = await _identityService.GetListAsync(board.Team.Adherents.Select(a => a.IdentityId), cancellationToken);
            var adherentDtos = _mapper.Map<IEnumerable<AdherentDto>>(board.Team.Adherents);    

            var boardDto = _mapper.Map<BoardDetailDto>(board);
            boardDto.Team.Adherents = _mapper.Map(users, adherentDtos);
            boardDto.Adherent = adherentDtos.First(a => a.Id == board.Adherent.Id);

            return boardDto;
        }
    }
}
