using Scrumboard.SharedKernel.Types;

namespace Scrumboard.SharedKernel.Entities;

/// <summary>
/// A base class for DDD Entities.
/// </summary>
public abstract class CreatedAtEntityBase<TId> : EntityBase<TId>, ICreatedAtEntity
    where TId : struct, IEquatable<TId>
{
    public UserId CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}
