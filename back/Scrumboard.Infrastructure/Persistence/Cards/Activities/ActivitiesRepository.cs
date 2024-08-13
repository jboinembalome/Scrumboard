using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;

namespace Scrumboard.Infrastructure.Persistence.Cards.Activities;

internal sealed class ActivitiesRepository(
    ScrumboardDbContext dbContext) : IActivitiesRepository
{
    public async Task<Activity> AddAsync(
        Activity activity, 
        CancellationToken cancellationToken = default)
    {
        await dbContext.Activities.AddAsync(activity, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return activity;
    }
    
    public async Task<IReadOnlyList<Activity>> AddAsync(
        IEnumerable<Activity> activities, 
        CancellationToken cancellationToken = default)
    {
        var entities = activities.ToArray();
        
        await dbContext.Activities.AddRangeAsync(entities, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return entities;
    }
}
