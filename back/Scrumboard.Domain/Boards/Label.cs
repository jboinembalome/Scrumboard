using Scrumboard.Domain.Common;
using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Boards;

public sealed class Label : AuditableEntityBase<LabelId>
{
    public string Name { get; set; }
    public Colour Colour { get; set; }
    public BoardId BoardId { get; set; }
}
