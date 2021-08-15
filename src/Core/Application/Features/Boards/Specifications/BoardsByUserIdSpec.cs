using Ardalis.Specification;
using Scrumboard.Domain.Entities;
using System;

namespace Scrumboard.Application.Specifications
{
    public class BoardsByUserIdSpec : Specification<Board>
    {
        public BoardsByUserIdSpec(string userId)
        {
            Query.Where(b => b.Adherent.IdentityGuid == userId)
                .Include(b => b.Adherent)
                .Include(b => b.Team)
                 .ThenInclude(t => t.Adherents)
                .Include(b => b.BoardSetting)
                 .ThenInclude(bs => bs.Colour);
        }
    }
}
