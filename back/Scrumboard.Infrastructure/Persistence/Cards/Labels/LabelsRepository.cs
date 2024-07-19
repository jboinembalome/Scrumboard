using AutoMapper;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Infrastructure.Persistence.Cards.Labels;

internal sealed class LabelsRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : ILabelsRepository
{
    public async Task<Label?> TryGetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var keyValues = new object[] { id };
        var dao = await dbContext.Labels.FindAsync(keyValues, cancellationToken);
        
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
        var keyValues = new object[] { labelEdition.Id };
        var dao = await dbContext.Labels.FindAsync(keyValues, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(dao);

        mapper.Map(labelEdition, dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<Label>(dao);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var keyValues = new object[] { id };
        var dao = await dbContext.Labels.FindAsync(keyValues, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(dao);
        
        dbContext.Labels.Remove(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
