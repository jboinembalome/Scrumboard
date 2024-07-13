using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

namespace Scrumboard.Application.Abstractions.Boards;

public interface IBoardsService
{
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<Board> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Board>> GetAsync(CancellationToken cancellationToken = default);
    Task<Board> AddAsync(BoardCreation boardCreation, CancellationToken cancellationToken = default);
    Task<Board> UpdateAsync(BoardEdition boardEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
