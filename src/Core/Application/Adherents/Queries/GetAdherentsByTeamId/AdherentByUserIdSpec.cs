using Ardalis.Specification;
using Scrumboard.Domain.Adherents;

namespace Scrumboard.Application.Adherents.Queries.GetAdherentsByTeamId;

internal sealed class AdherentByUserIdSpec : Specification<Adherent>
{
    public AdherentByUserIdSpec(string userId)
    {
        Query.Where(a => a.IdentityId == userId);
    }
}