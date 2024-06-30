using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Boards.ListBoards;

public interface IListBoardsRepository
{
    Task<ListBoard?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
}
