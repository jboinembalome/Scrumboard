using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Infrastructure.Persistence.ListBoards;

internal sealed class ListBoardsQueryRepository(
    ScrumboardDbContext dbContext) : IListBoardsQueryRepository
{
    public async Task<IReadOnlyList<ListBoard>> GetByBoardIdAsync(
        BoardId boardId, 
        bool? includeCards, 
        CancellationToken cancellationToken = default)
    {
        var query = Query();

        if (includeCards == true)
        {
            query = query
                .Include(l => l.Cards)
                    .ThenInclude(c => c.Labels)
                .Include(b => b.Cards)
                    .ThenInclude(c => c.Assignees);
        }

        return await query
            .Where(x => x.BoardId == boardId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ListBoard?> TryGetByIdAsync(
        ListBoardId id, 
        CancellationToken cancellationToken = default) 
        => await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    private IQueryable<ListBoard> Query() 
        => dbContext.ListBoards
            .AsNoTracking();
}
