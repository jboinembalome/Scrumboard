using Ardalis.Specification;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.Comments.Specifications;

internal sealed class CommentWithAdherentSpec : Specification<Comment>, ISingleResultSpecification<Comment>
{
    public CommentWithAdherentSpec(int commentId)
    {
        Query.Where(c => c.Id == commentId);
    }
}
