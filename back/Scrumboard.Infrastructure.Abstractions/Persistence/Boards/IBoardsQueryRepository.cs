using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

public interface IBoardsQueryRepository
{
    Task<Board?> TryGetByIdAsync(BoardId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Board>> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
}
