using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ITeamsRepository
{
    public async Task<Team?> TryGetByIdAsync(TeamId id, CancellationToken cancellationToken = default)
    {
        var keyValues = new object[] { id };
        var dao = await dbContext.Teams.FindAsync(keyValues, cancellationToken);
        
        return mapper.Map<Team>(dao);
    }

    public async Task<Team> AddAsync(TeamCreation teamCreation, CancellationToken cancellationToken = default)
    {
        var dao = mapper.Map<TeamDao>(teamCreation);
        
        dbContext.Teams.Add(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<Team>(dao);
    }

    public async Task<Team> UpdateAsync(TeamEdition teamEdition, CancellationToken cancellationToken = default)
    {
        var keyValues = new object[] { teamEdition.Id };
        var dao = await dbContext.Teams.FindAsync(keyValues, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(dao);

        mapper.Map(teamEdition, dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<Team>(dao);
    }

    public async Task DeleteAsync(TeamId id, CancellationToken cancellationToken = default)
    {
        var keyValues = new object[] { id };
        var dao = await dbContext.Teams.FindAsync(keyValues, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(dao);
        
        dbContext.Teams.Remove(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
