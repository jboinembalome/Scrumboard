using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

public interface ILabelsRepository
{
    Task<Label?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Label> AddAsync(LabelCreation labelCreation, CancellationToken cancellationToken = default);
    Task<Label> UpdateAsync(LabelEdition labelEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
