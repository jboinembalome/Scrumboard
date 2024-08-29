using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamsQueryRepository(
    ScrumboardDbContext dbContext) : ITeamsQueryRepository
{
    public async Task<Team?> TryGetByIdAsync(
        TeamId id, 
        CancellationToken cancellationToken = default) 
        => await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Team?> TryGetByBoardIdAsync(
        BoardId boardId, 
        CancellationToken cancellationToken = default) 
        => await Query()
            .FirstOrDefaultAsync(x => x.BoardId == boardId, cancellationToken);
    
    private IQueryable<Team> Query() 
        => dbContext.Teams
            .Include(x => x.Members)
            .AsNoTracking();
}
