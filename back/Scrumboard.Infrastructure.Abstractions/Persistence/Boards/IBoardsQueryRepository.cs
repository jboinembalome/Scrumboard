using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

public interface IBoardsQueryRepository
{
    Task<Board?> TryGetByIdAsync(BoardId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Board>> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
}
