using Ardalis.Specification;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Application.Teams.UpdateTeam;

public class TeamWithAdherentsSpec : Specification<Team>, ISingleResultSpecification
{
    public TeamWithAdherentsSpec(int teamId)
    {
        Query.Where(t => t.Id == teamId);
        Query.Include(a => a.Adherents);
    }
}