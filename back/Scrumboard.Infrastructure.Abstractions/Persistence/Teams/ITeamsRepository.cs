using Scrumboard.Domain.Teams;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

public interface ITeamsRepository
{
    Task<Team?> TryGetByIdAsync(TeamId id, CancellationToken cancellationToken = default);
    Task<Team> AddAsync(Team team, CancellationToken cancellationToken = default);
    Team Update(Team team);
    Task DeleteAsync(TeamId id, CancellationToken cancellationToken = default);
}
