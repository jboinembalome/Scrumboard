using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Specifications;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Commands.CreateBoard;

internal sealed class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, CreateBoardCommandResponse>
{
    private readonly IAsyncRepository<Board, int> _boardRepository;
    private readonly IAsyncRepository<Adherent, int> _adherentRepository;
    private readonly IMapper _mapper;

    public CreateBoardCommandHandler(
        IMapper mapper, 
        IAsyncRepository<Board, int> boardRepository, 
        IAsyncRepository<Adherent, int> adherentRepository)
    {
        _mapper = mapper;
        _boardRepository = boardRepository;
        _adherentRepository = adherentRepository;
    }

    public async Task<CreateBoardCommandResponse> Handle(
        CreateBoardCommand request, 
        CancellationToken cancellationToken)
    {
        var createBoardCommandResponse = new CreateBoardCommandResponse();

        var specification = new AdherentByUserIdSpec(request.UserId);
        var adherent = await _adherentRepository.FirstOrDefaultAsync(specification, cancellationToken);

        // TODO: Investigate
        if (adherent is null)
            await _adherentRepository.AddAsync(adherent, cancellationToken);

        var board = _mapper.Map<Board>(request);
        board.Adherent = adherent;
        board.BoardSetting = new BoardSetting();
        // TODO: Update code for team name
        board.Team = new Team { Name = "Team 1", Adherents = new Collection<Adherent>() };
        board.Team.Adherents.Add(adherent);

        board = await _boardRepository.AddAsync(board, cancellationToken);

        createBoardCommandResponse.Board = _mapper.Map<BoardDto>(board);

        return createBoardCommandResponse;
    }
}