using MediatR;

namespace Scrumboard.SharedKernel.Entities;

/// <summary>
/// A base type for domain events.
/// Includes DateOccurred which is set on creation.
/// </summary>
public abstract class DomainEventBase : INotification
{
    public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.Now;
}
