using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Abstractions.Boards;

public interface IBoardsService
{
    Task<bool> ExistsAsync(BoardId id, CancellationToken cancellationToken = default);
    Task<Board> GetByIdAsync(BoardId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Board>> GetAsync(CancellationToken cancellationToken = default);
    Task<Board> AddAsync(BoardCreation boardCreation, CancellationToken cancellationToken = default);
    Task<Board> UpdateAsync(BoardEdition boardEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(BoardId id, CancellationToken cancellationToken = default);
}
