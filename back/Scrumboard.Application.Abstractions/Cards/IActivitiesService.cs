using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;

namespace Scrumboard.Application.Abstractions.Cards;

public interface IActivitiesService
{
    Task<IReadOnlyList<Activity>> GetByCardIdAsync(CardId cardId, CancellationToken cancellationToken = default);
}
