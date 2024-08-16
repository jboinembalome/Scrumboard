using FluentValidation;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Application.Boards;

internal sealed class BoardsService(
    IBoardsRepository boardsRepository,
    IBoardsQueryRepository boardsQueryRepository,
    IValidator<BoardCreation> boardCreationValidator,
    IValidator<BoardEdition> boardEditionValidator,
    ICurrentUserService currentUserService) : IBoardsService
{
    public async Task<bool> ExistsAsync(
        BoardId id, 
        CancellationToken cancellationToken = default)
    {
        var board = await boardsRepository.TryGetByIdAsync(id, cancellationToken);

        return board is not null;
    }

    public async Task<Board> GetByIdAsync(
        BoardId id, 
        CancellationToken cancellationToken = default) 
        => await boardsQueryRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);

    public Task<IReadOnlyList<Board>> GetAsync(
        CancellationToken cancellationToken = default)
        => boardsQueryRepository.GetByOwnerIdAsync((OwnerId)currentUserService.UserId, cancellationToken);

    public async Task<Board> AddAsync(
        BoardCreation boardCreation, 
        CancellationToken cancellationToken = default)
    {
        boardCreation.OwnerId = (OwnerId)currentUserService.UserId;
        
        await boardCreationValidator.ValidateAndThrowAsync(boardCreation, cancellationToken);
        
        // TODO: Use Domain Event to create the team (BoardCreatedEvent)
        //var user = await identityService.GetUserAsync(currentUserService.UserId, cancellationToken);
        
        // var board = mapper.Map<Board>(request);
        // board.BoardSetting = new BoardSetting();
        // // TODO: Update code for team name
        // board.Team = new Team { Name = "Team 1", Members = [] };
        // board.Team.Members.Add(user.Id);
        
        return await boardsRepository.AddAsync(boardCreation, cancellationToken);
    }

    public async Task<Board> UpdateAsync(
        BoardEdition boardEdition, 
        CancellationToken cancellationToken = default)
    {
        await boardEditionValidator.ValidateAndThrowAsync(boardEdition, cancellationToken);
        
        return await boardsRepository.UpdateAsync(boardEdition, cancellationToken);
    }

    public async Task DeleteAsync(
        BoardId id, 
        CancellationToken cancellationToken = default)
    {
        await boardsRepository.TryGetByIdAsync(id, cancellationToken)
            .OrThrowResourceNotFoundAsync(id);
        
        await boardsRepository.DeleteAsync(id, cancellationToken);
    }
}
