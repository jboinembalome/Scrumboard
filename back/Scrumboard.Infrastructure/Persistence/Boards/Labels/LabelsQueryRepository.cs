using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Infrastructure.Persistence.Boards.Labels;

internal sealed class LabelsQueryRepository(
    ScrumboardDbContext dbContext) : ILabelsQueryRepository
{
    public async Task<IReadOnlyList<Label>> GetByBoardIdAsync(
        BoardId boardId, 
        CancellationToken cancellationToken = default) 
        => await Query()
            .Where(x => x.BoardId == boardId)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Label>> GetAsync(
        IEnumerable<LabelId> labelIds, 
        CancellationToken cancellationToken = default)
    {
        var idValues = labelIds
            .ToHashSet()
            .ToList();

        if (idValues.Count == 0)
        {
            return [];
        }

        return await Query()
            .Where(x => idValues.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<Label?> TryGetByIdAsync(
        LabelId id, 
        CancellationToken cancellationToken = default) 
        => await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    private IQueryable<Label> Query()
        => dbContext.Labels
            .AsNoTracking();
}
