using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.SharedKernel.Extensions;

namespace Scrumboard.Infrastructure.Persistence.Boards.Labels;

internal sealed class LabelsRepository(
    ScrumboardDbContext dbContext) : ILabelsRepository
{
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
        
        return await dbContext.Labels
            .Where(x => idValues.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<Label?> TryGetByIdAsync(
        LabelId id, 
        CancellationToken cancellationToken = default) 
        => await dbContext.Labels.FindAsync([id], cancellationToken);

    public async Task<Label> AddAsync(
        Label label, 
        CancellationToken cancellationToken = default)
    {
        await dbContext.Labels.AddAsync(label, cancellationToken);

        return label;
    }

    public Label Update(Label label)
    {
        dbContext.Labels.Update(label);
        
        return label;
    }

    public async Task DeleteAsync(
        LabelId id, 
        CancellationToken cancellationToken = default)
    {
        var label = await dbContext.Labels.FindAsync([id], cancellationToken)
            .OrThrowEntityNotFoundAsync();

        dbContext.Labels.Remove(label);
    }
}
