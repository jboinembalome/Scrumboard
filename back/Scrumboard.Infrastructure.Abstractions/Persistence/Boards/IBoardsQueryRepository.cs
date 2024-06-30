using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

public interface IBoardsQueryRepository
{
    Task<Board?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Board>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}
