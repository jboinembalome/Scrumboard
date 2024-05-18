using System.Linq;
using Ardalis.Specification;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.Labels.Queries.GetLabelsByBoardId;

public class AllLabelsInBoardSpec : Specification<Label>, ISingleResultSpecification
{
    public AllLabelsInBoardSpec(int boardId)
    {
        Query.Include(l => l.Cards)
            .ThenInclude(c => c.ListBoard)
            .ThenInclude(l => l.Board);

        Query.Where(l => l.Cards.Any(c => c.ListBoard.Board.Id == boardId)); 
    }
}