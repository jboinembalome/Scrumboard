using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Cards;

public readonly record struct CardId(int Value)
    : IStrongId<int, CardId>
{
    public static implicit operator int(CardId strongId)
        => strongId.Value;

    public static explicit operator CardId(int value)
        => new(value);
}
