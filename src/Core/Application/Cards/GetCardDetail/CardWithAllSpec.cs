using Ardalis.Specification;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.GetCardDetail;

public class CardWithAllSpec : Specification<Card>, ISingleResultSpecification
{
    public CardWithAllSpec(int cardId)
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

        Query.Include(b => b.Comments)
            .ThenInclude(c => c.Adherent);

        Query.Include(b => b.Activities);
    }
}