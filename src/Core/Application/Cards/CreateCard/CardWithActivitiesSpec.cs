using Ardalis.Specification;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.CreateCard;

public class CardWithActivitiesSpec : Specification<Card>, ISingleResultSpecification
{
    public CardWithActivitiesSpec(int cardId)
    {
        Query.Where(c => c.Id == cardId);

        Query.Include(b => b.Activities);
    }
}