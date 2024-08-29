using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Infrastructure.Persistence.Teams;

internal sealed class TeamsRepository(
    ScrumboardDbContext dbContext) : ITeamsRepository
{
    public async Task<Team?> TryGetByIdAsync(
        TeamId id, 
        CancellationToken cancellationToken = default)
    {
        var team = await dbContext.Teams.FindAsync([id], cancellationToken);
        
        if (team is not null)
        {
            await dbContext.Entry(team)
                .Collection(x => x.Members)
                .LoadAsync(cancellationToken);
        }
        
        return team;
    }

    public async Task<Team> AddAsync(
        Team team, 
        CancellationToken cancellationToken = default)
    {
        await dbContext.Teams.AddAsync(team, cancellationToken);

        return team;
    }

    public Team Update(Team team)
    {
        dbContext.Teams.Update(team);
        
        return team;
    }

    public async Task DeleteAsync(
        TeamId id, 
        CancellationToken cancellationToken = default)
    {
        var team = await dbContext.Teams.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
        dbContext.Teams.Remove(team);
    }
}
