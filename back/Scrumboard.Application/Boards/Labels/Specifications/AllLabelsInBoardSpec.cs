using Ardalis.Specification;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Boards.Labels.Specifications;

internal sealed class AllLabelsInBoardSpec : Specification<Board>, ISingleResultSpecification<Board>
{
    public AllLabelsInBoardSpec(int boardId)
    {
        Query.Include(x => x.ListBoards)
            .ThenInclude(y => y.Cards)
            .ThenInclude(z => z.Labels);

        Query.Where(x => x.Id == boardId 
                         && x.ListBoards
                             .Any(y => y.Cards
                                 .Any(z => z.Labels.Count > 0))); 
    }
}
