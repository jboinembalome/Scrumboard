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
        => await dbContext.Teams
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Team?> TryGetByBoardIdAsync(
        BoardId boardId, 
        CancellationToken cancellationToken = default) 
        => await dbContext.Teams
            .AsNoTracking()
            .Where(x => x.Id == boardId)
            .FirstOrDefaultAsync(cancellationToken);
}
