using Scrumboard.Domain.Cards.Activities;

namespace Scrumboard.Application.Abstractions.Cards;

public interface IActivitiesService
{
    Task<IReadOnlyList<Activity>> GetByCardIdAsync(int cardId, CancellationToken cancellationToken = default);
}
