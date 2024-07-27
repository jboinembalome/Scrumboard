using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

public interface IListBoardsQueryRepository
{
    Task<ListBoard?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
}
