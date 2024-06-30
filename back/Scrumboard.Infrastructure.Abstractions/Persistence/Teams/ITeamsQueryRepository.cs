using Scrumboard.Domain.Teams;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

public interface ITeamsQueryRepository
{
    Task<Team?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Team?> TryGetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default);
}
