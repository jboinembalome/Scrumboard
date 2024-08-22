using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

public interface IBoardsRepository
{
    Task<Board?> TryGetByIdAsync(BoardId id, CancellationToken cancellationToken = default);
    Task<Board> AddAsync(Board board, CancellationToken cancellationToken = default);
    Board Update(Board board);
    Task DeleteAsync(BoardId id, CancellationToken cancellationToken = default);
}
