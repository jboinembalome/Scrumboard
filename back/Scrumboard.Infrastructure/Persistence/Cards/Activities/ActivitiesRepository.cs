using AutoMapper;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;

namespace Scrumboard.Infrastructure.Persistence.Cards.Activities;

internal sealed class ActivitiesRepository(
    ScrumboardDbContext dbContext,
    IMapper mapper) : IActivitiesRepository
{
    public async Task<Activity> AddAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        var dao = mapper.Map<ActivityDao>(activity);
        
        dbContext.Activities.Add(dao);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<Activity>(dao);
    }
    
    public async Task<IReadOnlyList<Activity>> AddAsync(IEnumerable<Activity> activities, CancellationToken cancellationToken = default)
    {
        var daos = mapper.Map<IEnumerable<ActivityDao>>(activities);
        
        dbContext.Activities.AddRange(daos);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<IReadOnlyList<Activity>>(daos);
    }
}
