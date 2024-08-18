using System.Diagnostics.CodeAnalysis;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Cards;

public sealed class AssigneeId(
    string value) : StringStrongId<AssigneeId>(value), IStrongId<AssigneeId, string>
{
    [return: NotNullIfNotNull(nameof(id))]
    public static implicit operator AssigneeId?(string? id)
        => id is null ? null : new AssigneeId(id);
}
