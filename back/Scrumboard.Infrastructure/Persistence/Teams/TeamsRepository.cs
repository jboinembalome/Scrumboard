using AutoMapper;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ITeamsRepository
{
    public async Task<Team?> TryGetByIdAsync(
        TeamId id, 
        CancellationToken cancellationToken = default) 
        => await dbContext.Teams.FindAsync([id], cancellationToken);

    public async Task<Team> AddAsync(
        TeamCreation teamCreation, 
        CancellationToken cancellationToken = default)
    {
        var team = mapper.Map<Team>(teamCreation);
        
        await dbContext.Teams.AddAsync(team, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return team;
    }

    public async Task<Team> UpdateAsync(
        TeamEdition teamEdition, 
        CancellationToken cancellationToken = default)
    {
        var team = await dbContext.Teams.FindAsync([teamEdition.Id], cancellationToken)
            .OrThrowEntityNotFoundAsync();

        mapper.Map(teamEdition, team);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return team;
    }

    public async Task DeleteAsync(
        TeamId id, 
        CancellationToken cancellationToken = default)
    {
        var team = await dbContext.Teams.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
        dbContext.Teams.Remove(team);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
