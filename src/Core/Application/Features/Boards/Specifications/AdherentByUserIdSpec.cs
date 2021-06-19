using Ardalis.Specification;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Application.Specifications
{
    public class AdherentByUserIdSpec : Specification<Adherent>
    {
        public AdherentByUserIdSpec(string userId)
        {
            Query.Where(a => a.IdentityGuid == userId);
        }
    }
}
