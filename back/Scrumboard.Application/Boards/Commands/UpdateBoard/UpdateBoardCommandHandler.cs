using AutoMapper;
using MediatR;
using Scrumboard.Application.Boards.Specifications;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Application.ListBoards.Dtos;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Commands.UpdateBoard;

internal sealed class UpdateBoardCommandHandler(
    IMapper mapper,
    IAsyncRepository<Board, int> boardRepository)
    : IRequestHandler<UpdateBoardCommand, UpdateBoardCommandResponse>
{
    public async Task<UpdateBoardCommandResponse> Handle(
        UpdateBoardCommand request,
        CancellationToken cancellationToken)
    {
        var updateBoardCommandResponse = new UpdateBoardCommandResponse();

        var specification = new UpdateBoardSpec(request.BoardId);
        var boardToUpdate = await boardRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (boardToUpdate is null)
            throw new NotFoundException(nameof(Board), request.BoardId);

        mapper.Map(request, boardToUpdate, opt => opt.BeforeMap((s, d) => MoveCards(s, d)));

        await boardRepository.UpdateAsync(boardToUpdate, cancellationToken);

        updateBoardCommandResponse.ListBoards = mapper.Map<IEnumerable<ListBoardDto>>(boardToUpdate.ListBoards);

        return updateBoardCommandResponse;

        //return Unit.Value;
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
    private static void MoveCards(UpdateBoardCommand updateBoardCommand, Board board)
    {
        if (!updateBoardCommand.ListBoards.Any())
        {
            return;
        }

        // TODO: Refoctor this...
        foreach (var listBoardDto in updateBoardCommand.ListBoards)
        {
            // Loop only on old values, so the id of the card will not be zero.
            foreach (var cardDto in listBoardDto.Cards.Where(c => c.Id != 0))
            {
                // Checks if the card in the source is present in the listBoard in the destination
                var card = board.ListBoards.SelectMany(l => l.Cards).FirstOrDefault(c => c.Id == cardDto.Id);

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
