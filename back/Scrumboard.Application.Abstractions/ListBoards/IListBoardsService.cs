using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

namespace Scrumboard.Application.Abstractions.ListBoards;

public interface IListBoardsService
{
    Task<ListBoard> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ListBoard> AddAsync(ListBoardCreation listBoardCreation, CancellationToken cancellationToken = default);
    Task<ListBoard> UpdateAsync(ListBoardEdition listBoardEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
