using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Persistence.Boards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Persistence.Cards.Labels;

public sealed class LabelDao : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Colour Colour { get; set; }
    public int BoardId { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
