using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

public interface IBoardsRepository
{
    Task<Board?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Board> AddAsync(BoardCreation boardCreation, CancellationToken cancellationToken = default);
    Task<Board> UpdateAsync(BoardEdition boardEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
