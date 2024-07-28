using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

public interface ILabelsRepository
{
    Task<IReadOnlyList<Label>> GetAsync(IEnumerable<LabelId> labelIds, CancellationToken cancellationToken = default);
    Task<Label?> TryGetByIdAsync(LabelId id, CancellationToken cancellationToken = default);
    Task<Label> AddAsync(LabelCreation labelCreation, CancellationToken cancellationToken = default);
    Task<Label> UpdateAsync(LabelEdition labelEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(LabelId id, CancellationToken cancellationToken = default);
}
