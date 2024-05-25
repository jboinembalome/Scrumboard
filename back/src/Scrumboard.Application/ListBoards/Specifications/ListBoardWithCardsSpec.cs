using Ardalis.Specification;
using Scrumboard.Domain.ListBoards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.ListBoards.Specifications;

internal sealed class ListBoardWithCardsSpec : Specification<ListBoard>, ISingleResultSpecification<ListBoard>
{
    public ListBoardWithCardsSpec(int listBoardId)
    {
            Query.Where(l => l.Id == listBoardId);

            Query.Include(l => l.Cards);
        }
}
