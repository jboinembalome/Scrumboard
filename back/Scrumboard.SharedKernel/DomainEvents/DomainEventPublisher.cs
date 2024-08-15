using MediatR;

namespace Scrumboard.SharedKernel.DomainEvents;

public sealed class DomainEventPublisher(
    IPublisher publisher) : IDomainEventPublisher
{
    public async Task PublishAsync(
        IEnumerable<HasDomainEventsBase> entities,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            var domainEvents = entity.DomainEvents.ToArray();
            
            entity.ClearDomainEvents();
            
            foreach (var domainEvent in domainEvents)
            {
                await publisher.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
