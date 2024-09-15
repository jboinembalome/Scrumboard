using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.ListBoards;
using Scrumboard.SharedKernel.DomainEvents;
using Scrumboard.SharedKernel.Entities;

namespace Scrumboard.Domain.Cards.Events;

// CardUpdatedDomainEvent is a "Fat Domain Event" as opposed to a "Thin Domain Event."
//
// Here, we chose to include the old and new values of the modified properties
// because the activities associated to a card require a full history of changes.
//
// A "Thin Event" with only IDs would have been less suitable, as we would have lost
// information about the specific changes (before and after values).
//
// Additionally, this event is highly specific to card updates and is not intended for 
// general use across the domain. If the need evolves, this approach 
// can always be adjusted.
public sealed class CardUpdatedDomainEvent(
    CardId id,
    PropertyValueChange<string> name,
    PropertyValueChange<string?> description,
    PropertyValueChange<DateTimeOffset?> dueDate,
    PropertyValueChange<int> position,
    PropertyValueChange<ListBoardId> listBoardId,
    PropertyValueChange<IReadOnlyCollection<AssigneeId>> assigneeIds,
    PropertyValueChange<IReadOnlyCollection<LabelId>> labelIds) : DomainEventBase
{
    public CardId Id { get; } = id;
    public PropertyValueChange<string> Name { get; } = name;
    public PropertyValueChange<string?> Description { get; } = description;
    public PropertyValueChange<DateTimeOffset?> DueDate { get; } = dueDate;
    public PropertyValueChange<int> Position { get; } = position;
    public PropertyValueChange<ListBoardId> ListBoardId { get; } = listBoardId;
    public PropertyValueChange<IReadOnlyCollection<AssigneeId>> AssigneeIds { get; } = assigneeIds;
    public PropertyValueChange<IReadOnlyCollection<LabelId>> LabelIds { get; } = labelIds;
}
