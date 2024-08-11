using System.ComponentModel.DataAnnotations.Schema;

namespace Scrumboard.SharedKernel.Entities;

public abstract class HasDomainEventsBase
{
    private readonly List<DomainEventBase> _domainEvents = [];
    
    [NotMapped]
    public IReadOnlyCollection<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEventBase domainEvent) 
        => _domainEvents.Add(domainEvent);
    
    protected void ClearDomainEvents() => _domainEvents.Clear();
}
