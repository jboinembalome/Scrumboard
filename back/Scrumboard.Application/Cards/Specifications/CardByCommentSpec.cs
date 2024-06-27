using Ardalis.Specification;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.Specifications;

internal sealed class CardByCommentSpec : Specification<Card>, ISingleResultSpecification<Card>
{
    public CardByCommentSpec(int commentId)
    {
        Query.Include(x => x.Activities);
        Query.Include(x => x.Comments);
        
        Query.Where(x => x.Comments.Any(y => y.Id == commentId));
    }
}
