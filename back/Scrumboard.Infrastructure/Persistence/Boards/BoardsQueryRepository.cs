using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.SharedKernel.Entities;

namespace Scrumboard.Infrastructure.Persistence.Boards;

internal sealed class BoardsQueryRepository(
    ScrumboardDbContext dbContext) : IBoardsQueryRepository
{
    public async Task<Board?> TryGetByIdAsync(
        BoardId id, 
        CancellationToken cancellationToken = default) 
        => await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Board>> GetByUserIdAsync(
        UserId userId, 
        CancellationToken cancellationToken = default) 
        => await Query()
            .Where(b => b.CreatedBy == userId)
            .ToListAsync(cancellationToken);

    private IQueryable<Board> Query()
        => dbContext.Boards
            .AsNoTracking()
            .Include(b => b.BoardSetting);
}
