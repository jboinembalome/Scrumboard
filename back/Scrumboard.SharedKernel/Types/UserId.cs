namespace Scrumboard.SharedKernel.Types;

public sealed class UserId(
    string value) : StringStrongId<UserId>(value), IStrongId<UserId, string>
{
    public static explicit operator UserId(string value)
        => new(value);
}
