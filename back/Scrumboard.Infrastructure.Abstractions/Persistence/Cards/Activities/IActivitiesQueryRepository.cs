using Scrumboard.Domain.Cards.Activities;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;

public interface IActivitiesQueryRepository
{
    Task<IReadOnlyList<Activity>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default);
}
