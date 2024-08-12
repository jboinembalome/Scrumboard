using Scrumboard.SharedKernel.Types;

namespace Scrumboard.SharedKernel.Entities;

/// <summary>
/// A base class for DDD Entities.
/// </summary>
public abstract class CreatedAtEntityBase<TId> : HasDomainEventsBase, ICreatedAtEntity
    where TId : struct, IEquatable<TId>
{
    public TId Id { get; set; }
    
    public UserId CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}
