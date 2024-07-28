using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

public interface ILabelsQueryRepository
{
    Task<IReadOnlyList<Label>> GetByBoardIdAsync(BoardId boardId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Label>> GetAsync(IEnumerable<LabelId> labelIds, CancellationToken cancellationToken = default);
    Task<Label?> TryGetByIdAsync(LabelId id, CancellationToken cancellationToken = default);
}
