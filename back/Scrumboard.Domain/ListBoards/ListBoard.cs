using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.ListBoards;

public sealed class ListBoard
{
    public ListBoardId Id { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }
    public BoardId BoardId { get; set; }
    public ICollection<Card> Cards { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public UserId? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}
