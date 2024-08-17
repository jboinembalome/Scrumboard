using Scrumboard.SharedKernel.DomainEvents;

namespace Scrumboard.Domain.Boards.Events;

public sealed class BoardCreatedDomainEvent(
    BoardId boardId,
    OwnerId ownerId) : DomainEventBase
{
    public BoardId BoardId { get; } = boardId;
    public OwnerId OwnerId { get; } = ownerId;
}
