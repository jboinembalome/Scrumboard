using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.ListBoards;

public sealed class ListBoard : AuditableEntityBase<ListBoardId>
{
    public ListBoard() { }
    
    public ListBoard(string name, int position, BoardId boardId)
    {
        Name = name;
        Position = position;
        BoardId = boardId;
    }

    public string Name { get; private set; }
    public int Position { get; private set; }
    public BoardId BoardId { get; private set; }
    public IReadOnlyCollection<Card> Cards { get; private set; } = [];
    
    public void Update(
        string name, 
        int position)
    {
        Name = name;
        Position = position;
    }
}
