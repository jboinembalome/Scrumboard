using Scrumboard.Domain.Boards.Events;
using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Boards;

public sealed class Board : AuditableEntityBase<BoardId>
{
    public Board()
    {
        AddDomainEvent(new BoardCreatedDomainEvent(Id, OwnerId));
    }
    
    public string Name { get; init; }
    public bool IsPinned { get; init; }
    public BoardSetting BoardSetting { get; init; }
    public OwnerId OwnerId { get; init; }
}
