using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardsQueryRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : IBoardsQueryRepository
{
    public async Task<Board?> TryGetByIdAsync(
        BoardId id, 
        CancellationToken cancellationToken = default)
    {
        var dao = await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return mapper.Map<Board>(dao);
    }

    public async Task<IReadOnlyList<Board>> GetByUserIdAsync(
        UserId userId, 
        CancellationToken cancellationToken = default)
    {
        var daos = await Query()
            .Where(b => b.Team.Members.Any(a => a.MemberId == userId))
            .ToListAsync(cancellationToken);
        
        return mapper.Map<IReadOnlyList<Board>>(daos);
    }
    
    private IQueryable<BoardDao> Query()
        => dbContext.Boards
            .AsNoTracking()
            .AsSplitQuery()
            .Include(b => b.Team)
                .ThenInclude(t => t.Members)
            .Include(b => b.BoardSetting);
}
