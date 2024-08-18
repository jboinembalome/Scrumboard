using System.Diagnostics.CodeAnalysis;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Boards;

public sealed class BoardSettingId(
    int value) : IntStrongId<BoardSettingId>(value), IStrongId<BoardSettingId, int>
{
    [return: NotNullIfNotNull(nameof(id))]
    public static implicit operator BoardSettingId?(int? id)
        => id is null ? null : new BoardSettingId(id.Value);
}
