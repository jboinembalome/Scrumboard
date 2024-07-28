using Scrumboard.Domain.Cards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

public sealed class CardEdition : CardInputBase
{
    public CardId Id { get; set; }
}
