using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Application.ListBoards.Dtos;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Commands.UpdateBoard;

internal sealed class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand, UpdateBoardCommandResponse>
{
    private readonly IAsyncRepository<Board, int> _boardRepository;
    private readonly IMapper _mapper;

    public UpdateBoardCommandHandler(
        IMapper mapper, 
        IAsyncRepository<Board, int> boardRepository)
    {
        _mapper = mapper;
        _boardRepository = boardRepository;
    }

    public async Task<UpdateBoardCommandResponse> Handle(
        UpdateBoardCommand request, 
        CancellationToken cancellationToken)
    {
        var updateBoardCommandResponse = new UpdateBoardCommandResponse();

        var specification = new UpdateBoardSpec(request.BoardId);
        var boardToUpdate = await _boardRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (boardToUpdate is null)
            throw new NotFoundException(nameof(Board), request.BoardId);

        _mapper.Map(request, boardToUpdate, opt => opt.BeforeMap((s, d) => MoveCards(s, d)));

        await _boardRepository.UpdateAsync(boardToUpdate, cancellationToken);

        updateBoardCommandResponse.ListBoards = _mapper.Map<IEnumerable<ListBoardDto>>(boardToUpdate.ListBoards);

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
        if (updateBoardCommand.ListBoards is null || !updateBoardCommand.ListBoards.Any())
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

                if (card != null)
                    // Does the destination listBoard id match the source listBoard id? If not, card has been moved
                    if (card.ListBoard.Id != listBoardDto.Id)
                    {
                        // Now we need to move the card into the listBoard, so lets find the destination
                        // listBoard that the card should be moved into
                        var oldListBoard = card.ListBoard;
                        var newListBoard = board.ListBoards.FirstOrDefault(l => l.Id == listBoardDto.Id);

                        //  Finally, we remove the card from the card collection of the old listBoard table and
                        //  add it to the new listBoard.
                        oldListBoard.Cards.Remove(card);

                        if (newListBoard is not null)
                        {
                            card.ListBoard = newListBoard;

                            newListBoard.Cards.Add(card);
                        }
                    }
            }
        }
    }
}