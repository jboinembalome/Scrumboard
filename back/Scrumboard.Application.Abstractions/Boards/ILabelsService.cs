using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Application.Abstractions.Boards;

public interface ILabelsService
{
    Task<IReadOnlyList<Label>> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Label>> GetAsync(IEnumerable<int> labelIds, CancellationToken cancellationToken = default);
    Task<Label> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Label> AddAsync(LabelCreation labelCreation, CancellationToken cancellationToken = default);
    Task<Label> UpdateAsync(LabelEdition labelEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
