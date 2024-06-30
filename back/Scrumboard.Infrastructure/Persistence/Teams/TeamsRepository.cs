using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ITeamsRepository
{
    public async Task<Team?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var keyValues = new object[] { id };
        var dao = await dbContext.Teams.FindAsync(keyValues, cancellationToken);
        
        return mapper.Map<Team>(dao);
    }

    public async Task<Team> AddAsync(Team team, CancellationToken cancellationToken = default)
    {
        var dao = mapper.Map<TeamDao>(team);
        
        dbContext.Teams.Add(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<Team>(dao);
    }

    public async Task<Team> UpdateAsync(Team team, CancellationToken cancellationToken = default)
    {
        var keyValues = new object[] { team.Id };
        var dao = await dbContext.Teams.FindAsync(keyValues, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(dao);

        mapper.Map(team, dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<Team>(dao);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Teams.FirstAsync(x => x.Id == id, cancellationToken);
        
        dbContext.Teams.Remove(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
