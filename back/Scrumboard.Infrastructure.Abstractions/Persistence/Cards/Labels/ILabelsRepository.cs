using Scrumboard.Domain.Boards.Labels;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

public interface ILabelsRepository
{
    Task<IReadOnlyList<Label>> GetAsync(IEnumerable<LabelId> labelIds, CancellationToken cancellationToken = default);
    Task<Label?> TryGetByIdAsync(LabelId id, CancellationToken cancellationToken = default);
    Task<Label> AddAsync(Label label, CancellationToken cancellationToken = default);
    Label Update(Label label);
    Task DeleteAsync(LabelId id, CancellationToken cancellationToken = default);
}
