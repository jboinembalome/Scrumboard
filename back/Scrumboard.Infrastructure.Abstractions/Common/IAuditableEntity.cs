namespace Scrumboard.Infrastructure.Abstractions.Common;

public interface IAuditableEntity
{
    string CreatedBy { get; set; }
    DateTimeOffset CreatedDate { get; set; }
    string? LastModifiedBy { get; set; }
    DateTimeOffset? LastModifiedDate { get; set; }
}
