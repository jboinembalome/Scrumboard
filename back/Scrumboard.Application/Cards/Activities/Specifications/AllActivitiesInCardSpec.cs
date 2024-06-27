using Ardalis.Specification;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.Activities.Specifications;

internal sealed class AllActivitiesInCardSpec : Specification<Card>, ISingleResultSpecification<Card>
{
    public AllActivitiesInCardSpec(int cardId)
    {
        Query.Include(x => x.Activities);

        Query.Where(x => x.Id == cardId);
    }
}
