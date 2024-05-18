using Ardalis.Specification;
using Scrumboard.Domain.Adherents;

namespace Scrumboard.Application.Adherents.Queries.GetAdherentsByTeamId;

public class AdherentByUserIdSpec : Specification<Adherent>
{
    public AdherentByUserIdSpec(string userId)
    {
        Query.Where(a => a.IdentityId == userId);
    }
}