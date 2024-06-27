using System.Collections.ObjectModel;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Commands.CreateBoard;

internal sealed class CreateBoardCommandHandler(
    IMapper mapper,
    IAsyncRepository<Board, int> boardRepository,
    IIdentityService identityService)
    : IRequestHandler<CreateBoardCommand, CreateBoardCommandResponse>
{
    public async Task<CreateBoardCommandResponse> Handle(
        CreateBoardCommand request, 
        CancellationToken cancellationToken)
    {
        var createBoardCommandResponse = new CreateBoardCommandResponse();
        
        var adherent = await identityService.GetUserAsync(request.CreatorId, cancellationToken);
        
        var board = mapper.Map<Board>(request);
        board.BoardSetting = new BoardSetting();
        // TODO: Update code for team name
        board.Team = new Team { Name = "Team 1", Adherents = new Collection<Guid>() };
        board.Team.Adherents.Add(adherent.Id);

        board = await boardRepository.AddAsync(board, cancellationToken);

        createBoardCommandResponse.Board = mapper.Map<BoardDto>(board);

        return createBoardCommandResponse;
    }
}
