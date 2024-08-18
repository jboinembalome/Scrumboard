using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Cards;

public sealed class AssigneeId(
    string value) : StringStrongId<AssigneeId>(value), IStrongId<AssigneeId, string>
{
    public static explicit operator AssigneeId(string value)
        => new(value);
}
