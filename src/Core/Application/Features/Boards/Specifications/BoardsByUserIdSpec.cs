﻿using Ardalis.Specification;
using Scrumboard.Domain.Entities;
using System.Linq;

namespace Scrumboard.Application.Specifications
{
    public class BoardsByUserIdSpec : Specification<Board>
    {
        public BoardsByUserIdSpec(string userId)
        {
            Query.Where(b => b.Team.Adherents.Any(a => a.IdentityId == userId))
                .Include(b => b.Adherent)
                .Include(b => b.Team)
                 .ThenInclude(t => t.Adherents)
                .Include(b => b.BoardSetting)
                 .ThenInclude(bs => bs.Colour);
        }
    }
}
