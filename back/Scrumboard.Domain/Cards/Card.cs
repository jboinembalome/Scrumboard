using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards;

public sealed class Card
{
    public CardId Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool Suscribed { get; set; }
    public DateTime? DueDate { get; set; }
    public int Position { get; set; }
    public ListBoardId ListBoardId { get; set; }
    public ICollection<LabelId> LabelIds { get; set; }
    public ICollection<UserId> AssigneeIds { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserId? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
