using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Teams;

public readonly record struct MemberId(string Value)
    : IStrongId<string, MemberId>
{
    public static implicit operator string(MemberId strongId)
        => strongId.Value;

    public static explicit operator MemberId(string value)
        => new(value);
}
