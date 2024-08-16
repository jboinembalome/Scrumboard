using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Boards;

public sealed class Board : AuditableEntityBase<BoardId>
{
    public string Name { get; set; } = "Untitled Board";
    public bool IsPinned { get; set; }
    public BoardSetting BoardSetting { get; set; }
    public OwnerId OwnerId { get; set; }
}
