using MediatR;

namespace Scrumboard.SharedKernel.DomainEvents;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    
}
