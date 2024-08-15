using System.ComponentModel.DataAnnotations.Schema;

namespace Scrumboard.SharedKernel.DomainEvents;

public abstract class HasDomainEventsBase
{
    private readonly List<IDomainEvent> _domainEvents = [];
    
    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) 
        => _domainEvents.Add(domainEvent);
    
    internal void ClearDomainEvents() => _domainEvents.Clear();
}
