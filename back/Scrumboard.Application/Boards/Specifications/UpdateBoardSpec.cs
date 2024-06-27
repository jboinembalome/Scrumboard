using Ardalis.Specification;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Boards.Specifications;

internal sealed class UpdateBoardSpec : Specification<Board>, ISingleResultSpecification<Board>
{
    public UpdateBoardSpec(int boardId)
    {
        Query.Where(b => b.Id == boardId);

        Query.Include(b => b.BoardSetting)
            .ThenInclude(bs => bs.Colour);

        Query.Include(b => b.ListBoards)
            .ThenInclude(l => l.Cards)
            .ThenInclude(c => c.Labels);

        Query.Include(b => b.ListBoards)
            .ThenInclude(l => l.Cards)
            .ThenInclude(c => c.Assignees);

        Query.Include(b => b.Team);
    }
}
