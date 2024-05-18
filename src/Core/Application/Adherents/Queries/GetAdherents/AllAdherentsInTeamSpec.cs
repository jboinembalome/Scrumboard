using System.Linq;
using Ardalis.Specification;
using Scrumboard.Domain.Adherents;

namespace Scrumboard.Application.Adherents.Queries.GetAdherents;

public class AllAdherentsInTeamSpec : Specification<Adherent>, ISingleResultSpecification
{
    public AllAdherentsInTeamSpec(int teamId)
    {
        Query.Include(a => a.Teams);

        Query.Where(a => a.Teams.Any(t => t.Id == teamId)); 
    }
}