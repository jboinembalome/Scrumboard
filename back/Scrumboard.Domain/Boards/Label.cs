using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Boards;

public sealed class Label
{
    public LabelId Id { get; set; }
    public string Name { get; set; }
    public Colour Colour { get; set; }
    public BoardId BoardId { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public UserId? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}
