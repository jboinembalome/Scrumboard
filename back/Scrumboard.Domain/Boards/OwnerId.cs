using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Boards;

public readonly record struct OwnerId(string Value)
    : IStrongId<string, OwnerId>
{
    public static implicit operator string(OwnerId strongId)
        => strongId.Value;

    public static explicit operator OwnerId(string value)
        => new(value);
}
