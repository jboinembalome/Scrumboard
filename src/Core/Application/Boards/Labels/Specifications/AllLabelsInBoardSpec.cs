using Ardalis.Specification;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Boards.Labels.Specifications;

internal sealed class AllLabelsInBoardSpec : Specification<Label>, ISingleResultSpecification<Label>
{
    public AllLabelsInBoardSpec(int boardId)
    {
        Query.Include(l => l.Cards)
            .ThenInclude(c => c.ListBoard)
            .ThenInclude(l => l.Board);

        Query.Where(l => l.Cards.Any(c => c.ListBoard.Board.Id == boardId)); 
    }
}
