using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

public interface ITeamsQueryRepository
{
    Task<Team?> TryGetByIdAsync(TeamId id, CancellationToken cancellationToken = default);
    Task<Team?> TryGetByBoardIdAsync(BoardId boardId, CancellationToken cancellationToken = default);
}
