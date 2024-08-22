using Scrumboard.Domain.Cards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Abstractions.Cards;

public sealed class CardEdition : CardInputBase
{
    public CardId Id { get; set; }
}
