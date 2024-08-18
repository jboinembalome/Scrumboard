using Scrumboard.Domain.Boards.Events;
using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Boards;

public sealed class Board : AuditableEntityBase<BoardId>
{
    public Board() { }
    
    public Board(
        string name, 
        bool isPinned, 
        BoardSetting boardSetting, 
        OwnerId ownerId)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        IsPinned = isPinned;
        BoardSetting = boardSetting ?? throw new ArgumentNullException(nameof(boardSetting));
        OwnerId = ownerId;
        
        AddDomainEvent(new BoardCreatedDomainEvent(Id, OwnerId));
    }
    
    public Board(
        BoardId id,
        string name, 
        bool isPinned, 
        BoardSetting boardSetting)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        IsPinned = isPinned;
        BoardSetting = boardSetting ?? throw new ArgumentNullException(nameof(boardSetting));
    }
    
    public string Name { get; private set; }
    public bool IsPinned { get; private set; }
    public BoardSetting BoardSetting { get; private set; }
    public OwnerId OwnerId { get; private set; }
}
