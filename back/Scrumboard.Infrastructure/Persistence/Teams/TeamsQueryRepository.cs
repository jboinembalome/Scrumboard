using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamsQueryRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ITeamsQueryRepository
{
    public async Task<Team?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Teams
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return mapper.Map<Team>(dao);
    }

    public async Task<Team?> TryGetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Boards
            .AsNoTracking()
            .Include(x => x.Team)
            .Where(x => x.Id == boardId)
            .Select(x => x.Team)
            .FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<Team>(dao);
    }
}
