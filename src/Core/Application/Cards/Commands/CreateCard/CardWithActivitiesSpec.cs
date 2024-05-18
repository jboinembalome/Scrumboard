using Ardalis.Specification;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.Commands.CreateCard;

internal sealed class CardWithActivitiesSpec : Specification<Card>, ISingleResultSpecification
{
    public CardWithActivitiesSpec(int cardId)
    {
        Query.Where(c => c.Id == cardId);

        Query.Include(b => b.Activities);
    }
}