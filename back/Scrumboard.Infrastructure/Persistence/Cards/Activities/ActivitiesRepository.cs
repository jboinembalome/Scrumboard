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
        dbContext.Activities.Add(activity);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return activity;
    }
    
    public async Task<IReadOnlyList<Activity>> AddAsync(
        IEnumerable<Activity> activities, 
        CancellationToken cancellationToken = default)
    {
        var entities = activities.ToArray();
        
        dbContext.Activities.AddRange(entities);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return entities;
    }
}
