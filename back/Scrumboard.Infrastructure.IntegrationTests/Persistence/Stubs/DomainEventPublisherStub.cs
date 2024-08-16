using Scrumboard.SharedKernel.DomainEvents;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Stubs;

internal sealed class DomainEventPublisherStub : IDomainEventPublisher
{
    public Task PublishAsync(IEnumerable<HasDomainEventsBase> entities, CancellationToken cancellationToken = default) 
        => Task.CompletedTask;
}
