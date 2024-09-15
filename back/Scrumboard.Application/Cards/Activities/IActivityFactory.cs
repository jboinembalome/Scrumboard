using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Events;

namespace Scrumboard.Application.Cards.Activities;
internal interface IActivityFactory
{
    Task<IReadOnlyCollection<Activity>> CreateAsync(CardUpdatedDomainEvent domainEvent, CancellationToken cancellationToken);
}
