using Scrumboard.SharedKernel.Types;

namespace Scrumboard.SharedKernel.Entities;

/// <summary>
/// A base class for DDD Entities.
/// </summary>
public abstract class AuditableEntityBase<TId> : EntityBase<TId>, IAuditableEntity
    where TId : struct, IEquatable<TId>
{
    public UserId CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public UserId? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}
