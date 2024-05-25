using Ardalis.Specification;
using Scrumboard.Domain.Cards.Activities;

namespace Scrumboard.Application.Cards.Activities.Specifications;

internal sealed class AllActivitiesInCardSpec : Specification<Activity>, ISingleResultSpecification<Activity>
{
    public AllActivitiesInCardSpec(int cardId)
    {
        Query.Include(a => a.Card);
        Query.Include(a => a.Adherent);

        Query.Where(l => l.Card.Id == cardId);
    }
}
