using MediatR;

namespace Scrumboard.SharedKernel.DomainEvents;

public interface IDomainEvent : INotification
{
    DateTimeOffset DateOccurred { get; }
}
