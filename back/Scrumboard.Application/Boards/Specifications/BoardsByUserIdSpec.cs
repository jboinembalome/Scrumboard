using Ardalis.Specification;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Boards.Specifications;

internal sealed class BoardsByUserIdSpec : Specification<Board>
{
    public BoardsByUserIdSpec(Guid userId)
    {
        Query.Where(b => b.Team.Adherents.Any(a => a == userId))
            .Include(b => b.Team)
            .ThenInclude(t => t.Adherents)
            .Include(b => b.BoardSetting)
            .ThenInclude(bs => bs.Colour);
    }
}
