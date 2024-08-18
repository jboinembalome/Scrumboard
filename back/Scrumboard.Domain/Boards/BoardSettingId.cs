using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Boards;

public sealed class BoardSettingId(
    int value) : IntStrongId<BoardSettingId>(value), IStrongId<BoardSettingId, int>
{
    public static explicit operator BoardSettingId(int value)
        => new(value);
}
