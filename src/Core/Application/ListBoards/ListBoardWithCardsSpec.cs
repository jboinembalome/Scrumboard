using Ardalis.Specification;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Application.ListBoards
{
    public class ListBoardWithCardsSpec : Specification<ListBoard>, ISingleResultSpecification
    {
        public ListBoardWithCardsSpec(int listBoardId)
        {
            Query.Where(l => l.Id == listBoardId);

            Query.Include(l => l.Cards);
        }
    }
}
