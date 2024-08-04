using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;

namespace Scrumboard.Application.Cards.Activities;

internal sealed class ActivitiesService(
    IActivitiesQueryRepository activitiesQueryRepository) : IActivitiesService
{
    public Task<IReadOnlyList<Activity>> GetByCardIdAsync(
        CardId cardId, 
        CancellationToken cancellationToken = default) 
        => activitiesQueryRepository.GetByCardIdAsync(cardId, cancellationToken);
}
