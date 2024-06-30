using AutoMapper;
using MediatR;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.Boards.Commands.CreateBoard;

internal sealed class CreateBoardCommandHandler(
    IMapper mapper,
    IBoardsRepository boardsRepository,
    IIdentityService identityService,
    ICurrentUserService currentUserService)
    : IRequestHandler<CreateBoardCommand, CreateBoardCommandResponse>
{
    public async Task<CreateBoardCommandResponse> Handle(
        CreateBoardCommand request, 
        CancellationToken cancellationToken)
    {
        var createBoardCommandResponse = new CreateBoardCommandResponse();
        
        var adherent = await identityService.GetUserAsync(currentUserService.UserId, cancellationToken);
        
        var board = mapper.Map<Board>(request);
        board.BoardSetting = new BoardSetting();
        // TODO: Update code for team name
        board.Team = new Team { Name = "Team 1", Adherents = [] };
        board.Team.Adherents.Add(new Adherent{ Id = adherent.Id});

        board = await boardsRepository.AddAsync(board, cancellationToken);

        createBoardCommandResponse.Board = mapper.Map<BoardDto>(board);

        return createBoardCommandResponse;
    }
}
