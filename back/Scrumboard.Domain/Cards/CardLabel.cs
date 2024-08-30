using Scrumboard.Domain.Boards.Labels;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards;

public sealed class CardLabel
{
    public CardId CardId { get; set; }
    public LabelId LabelId { get; set; }
}
