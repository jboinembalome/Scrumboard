using Ardalis.Specification;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.Specifications;

internal sealed class CardWithAllExceptCommentSpec : Specification<Card>, ISingleResultSpecification<Card>
{
    public CardWithAllExceptCommentSpec(int cardId)
    {
        Query.Where(c => c.Id == cardId);

        Query.Include(b => b.Labels);

        Query.Include(c => c.Assignees);

        Query.Include(b => b.Checklists)
            .ThenInclude(l => l.ChecklistItems);

        Query.Include(b => b.Activities);
    }
}
