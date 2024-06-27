namespace Scrumboard.Domain.Common;

public interface IAuditableEntity
{
    Guid CreatedBy { get; set; }
    DateTime CreatedDate { get; set; }
    Guid? LastModifiedBy { get; set; }
    DateTime? LastModifiedDate { get; set; }
}
