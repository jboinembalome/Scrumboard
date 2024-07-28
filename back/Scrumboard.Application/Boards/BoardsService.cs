using FluentValidation;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.Boards;

internal sealed class BoardsService(
    IBoardsRepository boardsRepository,
    IBoardsQueryRepository boardsQueryRepository,
    IValidator<BoardEdition> boardEditionValidator,
    ICurrentUserService currentUserService) : IBoardsService
{
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var board = await boardsRepository.TryGetByIdAsync(id, cancellationToken);

        return board is not null;
    }

    public async Task<Board> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
        => await boardsQueryRepository.TryGetByIdAsync(id, cancellationToken) 
           ?? throw new NotFoundException(nameof(Board), id);

    public Task<IReadOnlyList<Board>> GetAsync(CancellationToken cancellationToken = default)
        => boardsQueryRepository.GetByUserIdAsync(currentUserService.UserId, cancellationToken);

    public Task<Board> AddAsync(BoardCreation boardCreation, CancellationToken cancellationToken = default)
    {
        //var user = await identityService.GetUserAsync(currentUserService.UserId, cancellationToken);
        
        // TODO: Move into infra
        
        // var board = mapper.Map<Board>(request);
        // board.BoardSetting = new BoardSetting();
        // // TODO: Update code for team name
        // board.Team = new Team { Name = "Team 1", Members = [] };
        // board.Team.Members.Add(user.Id);
        
        return boardsRepository.AddAsync(boardCreation, cancellationToken);
    }

    public async Task<Board> UpdateAsync(BoardEdition boardEdition, CancellationToken cancellationToken = default)
    {
        await boardEditionValidator.ValidateAndThrowAsync(boardEdition, cancellationToken);
        
        return await boardsRepository.UpdateAsync(boardEdition, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        _ = await boardsRepository.TryGetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Board), id);;
        
        await boardsRepository.DeleteAsync(id, cancellationToken);
    }
    
    /// <summary>
    /// Move cards in listBoards. This method is used for the mapping of AutoMapper. 
    /// </summary>
    /// <remarks>
    /// We will check if a card has been moved from one listBoard to another, and if so,
    /// move the card before the mapping takes place.
    /// This way AutoMapper.Collections will not mark the object as an orphan in the first place!
    /// </remarks>
    /// <param name="updateBoardCommand">Source of map.</param>
    /// <param name="board">Destination of map.</param>
    private static void MoveCards(Board updateBoardCommand, Board board)
    {
        if (!updateBoardCommand.ListBoards.Any())
        {
            return;
        }

        // TODO: Refactor this...
        foreach (var listBoardDto in updateBoardCommand.ListBoards)
        {
            // Loop only on old values, so the id of the card will not be zero.
            foreach (var cardDto in listBoardDto.Cards.Where(c => c.Id != 0))
            {
                // Checks if the card in the source is present in the listBoard in the destination
                var card = board.ListBoards
                    .SelectMany(l => l.Cards)
                    .FirstOrDefault(c => c.Id == cardDto.Id);

                if (card is null 
                    || card.ListBoardId == listBoardDto.Id)
                {
                    // If card is not found or already in the correct list, continue to the next card
                    continue;
                }
                
                // Now we need to move the card into the listBoard, so lets find the destination
                // listBoard that the card should be moved into
                var oldListBoard = board.ListBoards.FirstOrDefault(l => l.Id == card.ListBoardId);
                var newListBoard = board.ListBoards.FirstOrDefault(l => l.Id == listBoardDto.Id);

                // Remove the card from the old listBoard, if it exists
                oldListBoard?.Cards.Remove(card);

                if (newListBoard is null)
                {
                    // If the new listBoard is not found, continue to the next card
                    continue;
                }

                // Add the card to the new listBoard and update its ListBoardId
                card.ListBoardId = newListBoard.Id;
                newListBoard.Cards.Add(card);
            }
        }
    }
}
