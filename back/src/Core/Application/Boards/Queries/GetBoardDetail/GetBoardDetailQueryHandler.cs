using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.Boards.Specifications;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Queries.GetBoardDetail;

internal sealed class GetBoardDetailQueryHandler : IRequestHandler<GetBoardDetailQuery, BoardDetailDto>
{
    private readonly IAsyncRepository<Board, int> _boardRepository;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetBoardDetailQueryHandler(
        IMapper mapper, 
        IAsyncRepository<Board, int> boardRepository, 
        IIdentityService identityService)
    {
        _mapper = mapper;
        _boardRepository = boardRepository;
        _identityService = identityService;
    }

    public async Task<BoardDetailDto> Handle(
        GetBoardDetailQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new BoardWithAllSpec(request.BoardId);
        var board = await _boardRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (board is null)
            throw new NotFoundException(nameof(Board), request.BoardId);

        var userIds = board.Team.Adherents.Select(a => a.IdentityId);
        var users = await _identityService.GetListAsync(userIds, cancellationToken);
        
        var adherentDtos = _mapper.Map<IEnumerable<AdherentDto>>(board.Team.Adherents);    

        var boardDto = _mapper.Map<BoardDetailDto>(board);
        boardDto.Team.Adherents = _mapper.Map(users, adherentDtos);
        boardDto.Adherent = adherentDtos.First(a => a.Id == board.Adherent.Id);

        var adherentDtosInBoard = boardDto.ListBoards
            .SelectMany(l => l.Cards
                .SelectMany(c => c.Adherents
                    .Where(a => adherentDtos
                        .Any(ad => ad.Id == a.Id))));
        
        MapUsers(users, adherentDtosInBoard);
        
        return boardDto;
    }

    private void MapUsers(IEnumerable<IUser> users, IEnumerable<AdherentDto> adherents)
    {
        foreach (var adherent in adherents)
        {
            var user = users.FirstOrDefault(u => u.Id == adherent.IdentityId);
            if (user == null)
                continue;

            _mapper.Map(user, adherent);
        }
    }
}