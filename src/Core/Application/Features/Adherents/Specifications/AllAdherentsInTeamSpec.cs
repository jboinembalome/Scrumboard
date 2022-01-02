using Ardalis.Specification;
using Scrumboard.Domain.Entities;
using System.Linq;

namespace Scrumboard.Application.Features.Adherents.Specifications
{
    public class AllAdherentsInTeamSpec : Specification<Adherent>, ISingleResultSpecification
    {
        public AllAdherentsInTeamSpec(int teamId)
        {
            Query.Include(a => a.Teams);

            Query.Where(a => a.Teams.Any(t => t.Id == teamId)); 
        }
    }
}
