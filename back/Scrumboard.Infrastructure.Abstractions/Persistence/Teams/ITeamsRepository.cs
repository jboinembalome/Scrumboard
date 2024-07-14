using Scrumboard.Domain.Teams;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

public interface ITeamsRepository
{
    Task<Team?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Team> AddAsync(TeamCreation teamCreation, CancellationToken cancellationToken = default);
    Task<Team> UpdateAsync(TeamEdition teamEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
