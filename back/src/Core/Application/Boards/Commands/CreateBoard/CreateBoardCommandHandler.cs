using System.Collections.ObjectModel;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Adherents.Specifications;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Commands.CreateBoard;

internal sealed class CreateBoardCommandHandler(
    IMapper mapper,
    IAsyncRepository<Board, int> boardRepository,
    IAsyncRepository<Adherent, int> adherentRepository)
    : IRequestHandler<CreateBoardCommand, CreateBoardCommandResponse>
{
    public async Task<CreateBoardCommandResponse> Handle(
        CreateBoardCommand request, 
        CancellationToken cancellationToken)
    {
        var createBoardCommandResponse = new CreateBoardCommandResponse();

        var specification = new AdherentByUserIdSpec(request.UserId);
        var adherent = await adherentRepository.FirstOrDefaultAsync(specification, cancellationToken);

        // TODO: Investigate
        if (adherent is null)
            await adherentRepository.AddAsync(adherent!, cancellationToken);

        var board = mapper.Map<Board>(request);
        board.Adherent = adherent!;
        board.BoardSetting = new BoardSetting();
        // TODO: Update code for team name
        board.Team = new Team { Name = "Team 1", Adherents = new Collection<Adherent>() };
        board.Team.Adherents.Add(adherent!);

        board = await boardRepository.AddAsync(board, cancellationToken);

        createBoardCommandResponse.Board = mapper.Map<BoardDto>(board);

        return createBoardCommandResponse;
    }
}
