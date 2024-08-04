using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Persistence.Boards.Labels;

public sealed class LabelDao : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Colour Colour { get; set; }
    public int BoardId { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}
