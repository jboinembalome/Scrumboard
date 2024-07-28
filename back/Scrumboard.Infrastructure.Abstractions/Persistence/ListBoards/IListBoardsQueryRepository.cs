using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

public interface IListBoardsQueryRepository
{
    Task<IReadOnlyList<ListBoard>> GetByBoardIdAsync(BoardId boardId, bool? includeCards, CancellationToken cancellationToken = default);
    Task<ListBoard?> TryGetByIdAsync(ListBoardId id, CancellationToken cancellationToken = default);
}
