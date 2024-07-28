using Scrumboard.Domain.Teams;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

public interface ITeamsRepository
{
    Task<Team?> TryGetByIdAsync(TeamId id, CancellationToken cancellationToken = default);
    Task<Team> AddAsync(TeamCreation teamCreation, CancellationToken cancellationToken = default);
    Task<Team> UpdateAsync(TeamEdition teamEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(TeamId id, CancellationToken cancellationToken = default);
}
