using Ardalis.Specification;
using Scrumboard.Domain.Entities;
using System;

namespace Scrumboard.Application.Features.Boards.Specifications
{
    public class BoardWithAllSpec : Specification<Board>, ISingleResultSpecification
    {
        public BoardWithAllSpec(Guid boardId)
        {
            Query.Where(b => b.Id == boardId);
            Query.Include(b => b.Labels);

            Query.Include(b => b.ListBoards)
                    .ThenInclude(l => l.Cards)
                    .ThenInclude(c => c.Checklists)
                    .ThenInclude(c => c.ChecklistItems);

            Query.Include(b => b.ListBoards)
                    .ThenInclude(l => l.Cards)
                    .ThenInclude(c => c.Labels);

            Query.Include(b => b.ListBoards)
                    .ThenInclude(l => l.Cards)
                    .ThenInclude(c => c.Comments);

            Query.Include(b => b.ListBoards)
                   .ThenInclude(l => l.Cards)
                   .ThenInclude(c => c.Attachments);
        }
    }
}
