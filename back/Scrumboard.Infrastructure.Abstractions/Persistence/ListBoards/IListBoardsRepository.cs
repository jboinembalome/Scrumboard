using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

public interface IListBoardsRepository
{
    Task<ListBoard?> TryGetByIdAsync(ListBoardId id, CancellationToken cancellationToken = default);
    Task<ListBoard> AddAsync(ListBoard listBoard, CancellationToken cancellationToken = default);
    ListBoard Update(ListBoard listBoard);
    Task DeleteAsync(ListBoardId id, CancellationToken cancellationToken = default);
}
