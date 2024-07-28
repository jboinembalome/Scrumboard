using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Cards.Activities;

public readonly record struct ActivityId(int Value)
    : IStrongId<int, ActivityId>
{
    public static implicit operator int(ActivityId strongId)
        => strongId.Value;

    public static explicit operator ActivityId(int value)
        => new(value);
}
