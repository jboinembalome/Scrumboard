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
    {
        var dao = await dbContext.Teams.FindAsync([id], cancellationToken);
        
        return mapper.Map<Team>(dao);
    }

    public async Task<Team> AddAsync(
        TeamCreation teamCreation, 
        CancellationToken cancellationToken = default)
    {
        var dao = mapper.Map<TeamDao>(teamCreation);
        
        dbContext.Teams.Add(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<Team>(dao);
    }

    public async Task<Team> UpdateAsync(
        TeamEdition teamEdition, 
        CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Teams.FindAsync([teamEdition.Id], cancellationToken)
            .OrThrowEntityNotFoundAsync();

        mapper.Map(teamEdition, dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<Team>(dao);
    }

    public async Task DeleteAsync(
        TeamId id, 
        CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Teams.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();
        
        dbContext.Teams.Remove(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
