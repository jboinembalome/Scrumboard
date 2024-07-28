using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Infrastructure.Persistence.Boards.Labels;

internal sealed class LabelsQueryRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ILabelsQueryRepository
{
    public async Task<IReadOnlyList<Label>> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default)
    {
        var daos = await Query()
            .Where(x => x.BoardId == boardId)
            .ToListAsync(cancellationToken);

        return mapper.Map<IReadOnlyList<Label>>(daos);
    }

    public async Task<IReadOnlyList<Label>> GetAsync(IEnumerable<int> labelIds, CancellationToken cancellationToken = default)
    {
        var idValues = labelIds.ToHashSet();

        if (idValues.Count == 0)
        {
            return [];
        }

        var daos = await Query()
            .Where(x => idValues.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return mapper.Map<IReadOnlyList<Label>>(daos);
    }

    public async Task<Label?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var dao = await Query()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return mapper.Map<Label>(dao);
    }

    private IQueryable<LabelDao> Query()
        => dbContext.Labels
            .AsNoTracking();
}
