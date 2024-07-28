#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

public abstract class CardInputBase
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int Position { get; set; }
    public int ListBoardId { get; set; }
    public IEnumerable<int> LabelIds { get; set; }
    public IEnumerable<string> AssigneeIds { get; set; }
}
