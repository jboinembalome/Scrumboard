using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Infrastructure.Persistence.ListBoards;

internal sealed class ListBoardsQueryRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : IListBoardsQueryRepository
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
        
        var daos = await query
            .Where(x => x.BoardId == boardId)
            .ToListAsync(cancellationToken);

        return mapper.Map<IReadOnlyList<ListBoard>>(daos);
    }

    public async Task<ListBoard?> TryGetByIdAsync(ListBoardId id, CancellationToken cancellationToken = default)
    {
        var dao = await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return mapper.Map<ListBoard>(dao);
    }
    
    private IQueryable<ListBoardDao> Query() 
        => dbContext.ListBoards
            .AsNoTracking();
}
