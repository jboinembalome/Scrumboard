using Ardalis.Specification;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Application.Features.Cards.Specifications
{
    public class CardWithActivitiesSpec : Specification<Card>, ISingleResultSpecification
    {
        public CardWithActivitiesSpec(int cardId)
        {
            Query.Where(c => c.Id == cardId);

            Query.Include(b => b.Activities);
        }
    }
}
