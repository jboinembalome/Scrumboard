using Scrumboard.Domain.Cards.Events;
using Scrumboard.SharedKernel.DomainEvents;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;

namespace Scrumboard.Application.Cards.Activities.DomainEventHandlers;

internal sealed class CreateActivitiesWhenCardUpdatedDomainEventHandler(
    IActivityFactory activityFactory,
    IActivitiesRepository activitiesRepository) : IDomainEventHandler<CardUpdatedDomainEvent>
{
    public async Task Handle(CardUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var newActivities = await activityFactory.CreateAsync(domainEvent, cancellationToken);

        if (newActivities.Count > 0)
        {
            await activitiesRepository.AddAsync(newActivities, cancellationToken);
        }
    }
}
