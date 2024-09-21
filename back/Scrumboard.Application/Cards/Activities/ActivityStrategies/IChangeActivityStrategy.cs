using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.SharedKernel.Entities;

namespace Scrumboard.Application.Cards.Activities.ActivityStrategies;

internal interface IChangeActivityStrategy<TProperty>
{
    Task<IReadOnlyCollection<Activity>> GetChangeActivitiesAsync(CardId cardId, PropertyValueChange<TProperty> change, CancellationToken cancellationToken);
}
