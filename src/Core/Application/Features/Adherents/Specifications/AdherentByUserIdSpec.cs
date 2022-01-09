using Ardalis.Specification;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Application.Features.Adherents.Specifications
{
    public class AdherentByUserIdSpec : Specification<Adherent>
    {
        public AdherentByUserIdSpec(string userId)
        {
            Query.Where(a => a.IdentityId == userId);
        }
    }
}
