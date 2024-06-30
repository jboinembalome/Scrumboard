using Scrumboard.Domain.Cards.Activities;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;

public interface IActivitiesRepository
{
    Task<Activity> AddAsync(Activity activity, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<Activity>> AddAsync(IEnumerable<Activity> activities, CancellationToken cancellationToken = default);
}
