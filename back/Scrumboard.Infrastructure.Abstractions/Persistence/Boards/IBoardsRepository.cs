using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

public interface IBoardsRepository
{
    Task<Board?> TryGetByIdAsync(BoardId id, CancellationToken cancellationToken = default);
    Task<Board> AddAsync(BoardCreation boardCreation, CancellationToken cancellationToken = default);
    Task<Board> UpdateAsync(BoardEdition boardEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(BoardId id, CancellationToken cancellationToken = default);
}
