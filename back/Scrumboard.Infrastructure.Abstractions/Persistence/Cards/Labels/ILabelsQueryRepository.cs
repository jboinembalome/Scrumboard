using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

public interface ILabelsQueryRepository
{
    Task<IReadOnlyList<Label>> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default);
    Task<Label?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
}
