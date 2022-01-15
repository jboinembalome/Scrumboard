using Ardalis.Specification;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Application.Features.Cards.Specifications
{
    public class CardWithAllExceptComment : Specification<Card>, ISingleResultSpecification
    {
        public CardWithAllExceptComment(int cardId)
        {
            Query.Where(c => c.Id == cardId);

            Query.Include(b => b.ListBoard)
                .ThenInclude(l => l.Board);

            Query.Include(b => b.Labels)
                .ThenInclude(l => l.Cards);

            Query.Include(c => c.Adherents)
                .ThenInclude(l => l.Cards);
            
            Query.Include(b => b.Attachments);

            Query.Include(b => b.Checklists)
                    .ThenInclude(l => l.ChecklistItems);
        }
    }
}
