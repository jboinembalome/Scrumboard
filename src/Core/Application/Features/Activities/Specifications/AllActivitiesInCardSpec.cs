using Ardalis.Specification;
using Scrumboard.Domain.Entities;
using System.Linq;

namespace Scrumboard.Application.Features.Activities.Specifications
{
    public class AllActivitiesInCardSpec : Specification<Activity>, ISingleResultSpecification
    {
        public AllActivitiesInCardSpec(int cardId)
        {
            Query.Include(a => a.Card);
            Query.Include(a => a.Adherent);

            Query.Where(l => l.Card.Id == cardId);
        }
    }
}
