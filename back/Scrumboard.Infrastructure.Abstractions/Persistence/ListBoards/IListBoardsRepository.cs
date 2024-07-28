using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

public interface IListBoardsRepository
{
    Task<ListBoard?> TryGetByIdAsync(ListBoardId id, CancellationToken cancellationToken = default);
    Task<ListBoard> AddAsync(ListBoardCreation listBoardCreation, CancellationToken cancellationToken = default);
    Task<ListBoard> UpdateAsync(ListBoardEdition listBoardEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(ListBoardId id, CancellationToken cancellationToken = default);
}
