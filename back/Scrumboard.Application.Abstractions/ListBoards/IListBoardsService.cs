using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Application.Abstractions.ListBoards;

public interface IListBoardsService
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ListBoard>> GetByBoardIdAsync(int boardId, bool? includeCards, CancellationToken cancellationToken = default);
    Task<ListBoard> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ListBoard> AddAsync(ListBoardCreation listBoardCreation, CancellationToken cancellationToken = default);
    Task<ListBoard> UpdateAsync(ListBoardEdition listBoardEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
