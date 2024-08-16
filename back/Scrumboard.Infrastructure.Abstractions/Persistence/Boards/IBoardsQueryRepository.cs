using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

public interface IBoardsQueryRepository
{
    Task<Board?> TryGetByIdAsync(BoardId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Board>> GetByOwnerIdAsync(OwnerId ownerId, CancellationToken cancellationToken = default);
}
