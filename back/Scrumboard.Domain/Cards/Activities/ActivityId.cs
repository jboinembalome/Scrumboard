using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Cards.Activities;

public sealed class ActivityId(
    int value) : IntStrongId<ActivityId>(value), IStrongId<ActivityId, int>
{
    public static explicit operator ActivityId(int value)
        => new(value);
}
