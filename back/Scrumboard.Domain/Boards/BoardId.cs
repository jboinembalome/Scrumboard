using System.Diagnostics.CodeAnalysis;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Boards;

public sealed class BoardId(
    int value) : IntStrongId<BoardId>(value), IStrongId<BoardId, int>
{
    [return: NotNullIfNotNull(nameof(id))]
    public static implicit operator BoardId?(int? id)
        => id is null ? null : new BoardId(id.Value);
}
