using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Boards;

public sealed class LabelId(
    int value) : IntStrongId<LabelId>(value), IStrongId<LabelId, int>
{
    public static explicit operator LabelId(int value)
        => new(value);
}
