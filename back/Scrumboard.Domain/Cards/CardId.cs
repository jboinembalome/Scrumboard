using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Cards;

public sealed class CardId(
    int value) : IntStrongId<CardId>(value), IStrongId<CardId, int>
{
    public static explicit operator CardId(int value)
        => new(value);
}
