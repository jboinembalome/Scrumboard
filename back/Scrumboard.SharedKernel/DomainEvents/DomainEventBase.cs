namespace Scrumboard.SharedKernel.DomainEvents;

/// <summary>
/// A base type for domain events.
/// Includes DateOccurred which is set on creation.
/// </summary>
public abstract class DomainEventBase : IDomainEvent
{
    public DateTimeOffset DateOccurred { get; } = DateTimeOffset.Now;
}
