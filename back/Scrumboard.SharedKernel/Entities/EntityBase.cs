using Scrumboard.SharedKernel.DomainEvents;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.SharedKernel.Entities;

/// <summary>
/// A base class for DDD Entities.
/// </summary>
public abstract class EntityBase<TId> : HasDomainEventsBase, IEntity
    where TId : notnull
{
    public TId Id { get; set; }
}
