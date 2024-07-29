using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Infrastructure.Persistence.Boards.Labels;

internal sealed class LabelsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ILabelsRepository
{
    public async Task<IReadOnlyList<Label>> GetAsync(
        IEnumerable<LabelId> labelIds,
        CancellationToken cancellationToken = default)
    {
        var idValues = labelIds
            .ToHashSet()
            .Select(x => x.Value)
            .ToList();

        if (idValues.Count == 0)
        {
            return [];
        }

        var daos = await dbContext.Labels
            .Where(x => idValues.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return mapper.Map<IReadOnlyList<Label>>(daos);
    }

    public async Task<Label?> TryGetByIdAsync(LabelId id, CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Labels.FindAsync([id], cancellationToken);

        return mapper.Map<Label>(dao);
    }

    public async Task<Label> AddAsync(LabelCreation labelCreation, CancellationToken cancellationToken = default)
    {
        var dao = mapper.Map<LabelDao>(labelCreation);

        dbContext.Labels.Add(dao);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<Label>(dao);
    }

    public async Task<Label> UpdateAsync(LabelEdition labelEdition, CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Labels.FindAsync([labelEdition.Id], cancellationToken);

        ArgumentNullException.ThrowIfNull(dao);

        mapper.Map(labelEdition, dao);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<Label>(dao);
    }

    public async Task DeleteAsync(LabelId id, CancellationToken cancellationToken = default)
    {
        var dao = await dbContext.Labels.FindAsync([id], cancellationToken);

        ArgumentNullException.ThrowIfNull(dao);

        dbContext.Labels.Remove(dao);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
