using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Boards;

public readonly record struct LabelId(int Value) 
    : IStrongId<int, LabelId>
{
    public static implicit operator int(LabelId strongId) 
        => strongId.Value;

    public static explicit operator LabelId(int value) 
        => new(value);
}
