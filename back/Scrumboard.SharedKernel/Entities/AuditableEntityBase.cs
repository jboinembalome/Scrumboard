using Scrumboard.SharedKernel.Types;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.SharedKernel.Entities;

/// <summary>
/// A base class for DDD Entities.
/// </summary>
public abstract class AuditableEntityBase<TId> : EntityBase<TId>, IAuditableEntity
    where TId : class
{
    public UserId CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public UserId? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}
