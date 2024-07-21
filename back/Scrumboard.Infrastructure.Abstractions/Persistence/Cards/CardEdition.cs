#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards;

public sealed class CardEdition : CardInputBase
{
    public int Id { get; set; }
}
