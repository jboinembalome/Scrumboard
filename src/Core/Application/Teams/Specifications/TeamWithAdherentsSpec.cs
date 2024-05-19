using Ardalis.Specification;
using Scrumboard.Domain.Teams;

namespace Scrumboard.Application.Teams.Specifications;

internal sealed class TeamWithAdherentsSpec : Specification<Team>, ISingleResultSpecification
{
    public TeamWithAdherentsSpec(int teamId)
    {
        Query.Where(t => t.Id == teamId);
        Query.Include(a => a.Adherents);
    }
}