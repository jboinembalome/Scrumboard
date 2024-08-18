using System.Diagnostics.CodeAnalysis;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Teams;

public sealed class MemberId(
    string value) : StringStrongId<MemberId>(value), IStrongId<MemberId, string>
{
    [return: NotNullIfNotNull(nameof(id))]
    public static implicit operator MemberId?(string? id)
        => id is null ? null : new MemberId(id);
}
