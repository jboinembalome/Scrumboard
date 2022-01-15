using Ardalis.Specification;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Application.Features.Comments.Specifications
{
    public class CommentWithAdherentSpec : Specification<Comment>, ISingleResultSpecification
    {
        public CommentWithAdherentSpec(int commentId)
        {
            Query.Where(c => c.Id == commentId);

            Query.Include(c => c.Adherent);
        }
    }
}
