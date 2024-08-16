using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Cards;

public readonly record struct AssigneeId(string Value)
    : IStrongId<string, AssigneeId>
{
    public static implicit operator string(AssigneeId strongId)
        => strongId.Value;

    public static explicit operator AssigneeId(string value)
        => new(value);
}
