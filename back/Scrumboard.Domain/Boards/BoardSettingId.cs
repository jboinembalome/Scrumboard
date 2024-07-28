using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Boards;

public readonly record struct BoardSettingId(int Value) 
    : IStrongId<int, BoardSettingId>
{
    public static implicit operator int(BoardSettingId strongId) 
        => strongId.Value;

    public static explicit operator BoardSettingId(int value) 
        => new(value);
}
