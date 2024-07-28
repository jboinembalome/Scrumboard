using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

public interface IListBoardsQueryRepository
{
    Task<IReadOnlyList<ListBoard>> GetByBoardIdAsync(int boardId, bool? includeCards, CancellationToken cancellationToken = default);
    Task<ListBoard?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
}
