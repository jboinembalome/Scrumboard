using Ardalis.Specification;
using Scrumboard.Domain.Adherents;

namespace Scrumboard.Application.Adherents.Specifications;

internal sealed class AdherentByUserIdSpec : Specification<Adherent>
{
    public AdherentByUserIdSpec(string userId)
    {
        Query.Where(a => a.IdentityId == userId);
    }
}