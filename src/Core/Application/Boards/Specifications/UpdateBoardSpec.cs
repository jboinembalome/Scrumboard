using Ardalis.Specification;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Boards.Specifications;

public class UpdateBoardSpec : Specification<Board>, ISingleResultSpecification
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
            .ThenInclude(c => c.Adherents);
    }
}