using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Teams;

public sealed class MemberId(
    string value) : StringStrongId<MemberId>(value), IStrongId<MemberId, string>
{
    public static explicit operator MemberId(string value)
        => new(value);
}
