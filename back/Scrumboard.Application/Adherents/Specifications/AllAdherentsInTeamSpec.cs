using Ardalis.Specification;
using Scrumboard.Domain.Teams;

namespace Scrumboard.Application.Adherents.Specifications;

internal sealed class AllAdherentsInTeamSpec : Specification<Team>, ISingleResultSpecification<Team>
{
    public AllAdherentsInTeamSpec(int teamId)
    {
        Query.Include(x => x.Adherents);

        Query.Where(x => x.Id == teamId); 
    }
}
