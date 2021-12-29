using Ardalis.Specification;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Application.Features.Cards.Specifications
{
    public class CardWithAllSpec : Specification<Card>, ISingleResultSpecification
    {
        public CardWithAllSpec(int cardId)
        {
            Query.Where(c => c.Id == cardId);

            Query.Include(b => b.ListBoard)
                .ThenInclude(l => l.Board)
                .ThenInclude(b => b.Adherent);

            Query.Include(b => b.Labels)
                .ThenInclude(l => l.Cards);

            Query.Include(c => c.Adherents);
            
            Query.Include(b => b.Attachments);

            Query.Include(b => b.Checklists)
                    .ThenInclude(l => l.ChecklistItems);

            Query.Include(b => b.Comments);
        }
    }
}
