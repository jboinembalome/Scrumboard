using Ardalis.Specification;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.Commands.UpdateCard;

internal sealed class CardWithAllExceptCommentSpec : Specification<Card>, ISingleResultSpecification
{
    public CardWithAllExceptCommentSpec(int cardId)
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

        Query.Include(b => b.Activities);
    }
}