using System.Diagnostics.CodeAnalysis;

namespace Scrumboard.SharedKernel.Types;

public sealed class UserId(
    string value) : StringStrongId<UserId>(value), IStrongId<UserId, string>
{
    [return: NotNullIfNotNull(nameof(id))]
    public static implicit operator UserId?(string? id)
        => id is null ? null : new UserId(id);
}
