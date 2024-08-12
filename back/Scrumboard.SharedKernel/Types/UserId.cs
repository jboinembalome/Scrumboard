namespace Scrumboard.SharedKernel.Types;

public readonly record struct UserId(string Value)
    : IStrongId<string, UserId>
{
    public static implicit operator string(UserId strongId)
        => strongId.Value;

    public static explicit operator UserId(string value)
        => new(value);
}
