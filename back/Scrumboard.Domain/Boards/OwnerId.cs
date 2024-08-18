using System.Diagnostics.CodeAnalysis;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Boards;

public sealed class OwnerId(
    string value) : StringStrongId<OwnerId>(value), IStrongId<OwnerId, string>
{
    [return: NotNullIfNotNull(nameof(id))]
    public static implicit operator OwnerId?(string? id)
        => id is null ? null : new OwnerId(id);
}
