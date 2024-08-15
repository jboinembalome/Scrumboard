using Scrumboard.SharedKernel.DomainEvents;

namespace Scrumboard.SharedKernel.Entities;

/// <summary>
/// A base class for DDD Entities.
/// </summary>
public abstract class EntityBase<TId> : HasDomainEventsBase, IEntity
    where TId : struct, IEquatable<TId>
{
    public TId Id { get; set; }
}
