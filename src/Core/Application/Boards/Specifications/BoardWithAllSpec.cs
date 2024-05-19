using Ardalis.Specification;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Boards.Specifications;

internal sealed class BoardWithAllSpec : Specification<Board>, ISingleResultSpecification
{
    public BoardWithAllSpec(int boardId)
    {
        Query.Where(b => b.Id == boardId);

        Query.Include(b => b.BoardSetting)
            .ThenInclude(bs => bs.Colour);

        Query.Include(b => b.ListBoards)
            .ThenInclude(l => l.Cards)
            .ThenInclude(c => c.Checklists)
            .ThenInclude(c => c.ChecklistItems);

        Query.Include(b => b.ListBoards)
            .ThenInclude(l => l.Cards)
            .ThenInclude(c => c.Labels);

        Query.Include(b => b.ListBoards)
            .ThenInclude(l => l.Cards)
            .ThenInclude(c => c.Adherents);

        Query.Include(b => b.ListBoards)
            .ThenInclude(l => l.Cards)
            .ThenInclude(c => c.Comments);

        Query.Include(b => b.ListBoards)
            .ThenInclude(l => l.Cards)
            .ThenInclude(c => c.Attachments);

        Query.Include(b => b.Team)
            .ThenInclude(t => t.Adherents);

        Query.Include(b => b.Adherent);
    }
}