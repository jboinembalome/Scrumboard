using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Boards;

public sealed class OwnerId(
    string value) : StringStrongId<OwnerId>(value), IStrongId<OwnerId, string>
{
    public static explicit operator OwnerId(string value)
        => new(value);
}
