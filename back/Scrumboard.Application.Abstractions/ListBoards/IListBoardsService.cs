using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Application.Abstractions.ListBoards;

public interface IListBoardsService
{
    Task<bool> ExistsAsync(ListBoardId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ListBoard>> GetByBoardIdAsync(BoardId boardId, bool? includeCards, CancellationToken cancellationToken = default);
    Task<ListBoard> GetByIdAsync(ListBoardId id, CancellationToken cancellationToken = default);
    Task<ListBoard> AddAsync(ListBoardCreation listBoardCreation, CancellationToken cancellationToken = default);
    Task<ListBoard> UpdateAsync(ListBoardEdition listBoardEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(ListBoardId id, CancellationToken cancellationToken = default);
}
