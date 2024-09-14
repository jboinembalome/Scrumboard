using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Abstractions.Cards;

public abstract class CardInputBase
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    public int Position { get; set; }
    public ListBoardId ListBoardId { get; set; }
    public IReadOnlyCollection<LabelId> LabelIds { get; set; }
    public IReadOnlyCollection<AssigneeId> AssigneeIds { get; set; }
}
