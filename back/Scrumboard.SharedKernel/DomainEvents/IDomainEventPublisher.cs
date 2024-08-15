namespace Scrumboard.SharedKernel.DomainEvents;

public interface IDomainEventPublisher
{
    Task PublishAsync(IEnumerable<HasDomainEventsBase> entities, CancellationToken cancellationToken = default);
}
