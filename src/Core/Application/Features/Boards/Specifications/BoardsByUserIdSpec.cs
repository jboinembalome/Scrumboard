using Ardalis.Specification;
using Scrumboard.Domain.Entities;
using System;

namespace Scrumboard.Application.Specifications
{
    public class BoardsByUserIdSpec : Specification<Board>
    {
        public BoardsByUserIdSpec(Guid userId)
        {
            Query.Where(b => b.UserId == userId);
        }
    }
}
