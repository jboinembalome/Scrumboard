using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Abstractions.Boards.Labels;

public interface ILabelsService
{
    Task<IReadOnlyList<Label>> GetByBoardIdAsync(BoardId boardId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Label>> GetAsync(IEnumerable<LabelId> labelIds, CancellationToken cancellationToken = default);
    Task<Label> GetByIdAsync(LabelId id, CancellationToken cancellationToken = default);
    Task<Label> AddAsync(LabelCreation labelCreation, CancellationToken cancellationToken = default);
    Task<Label> UpdateAsync(LabelEdition labelEdition, CancellationToken cancellationToken = default);
    Task DeleteAsync(LabelId id, CancellationToken cancellationToken = default);
}
