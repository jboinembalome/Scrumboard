using System.Linq;
using Ardalis.Specification;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Boards.Labels.Queries.GetLabelsByBoardId;

internal sealed class AllLabelsInBoardSpec : Specification<Label>, ISingleResultSpecification
{
    public AllLabelsInBoardSpec(int boardId)
    {
        Query.Include(l => l.Cards)
            .ThenInclude(c => c.ListBoard)
            .ThenInclude(l => l.Board);

        Query.Where(l => l.Cards.Any(c => c.ListBoard.Board.Id == boardId)); 
    }
}