using Scrumboard.SharedKernel.DomainEvents;

namespace Scrumboard.Domain.Cards.Events;

public sealed class CardUpdatedDomainEvent(
    CardId cardId) : DomainEventBase
{
    public CardId CardId { get; } = cardId;
}
