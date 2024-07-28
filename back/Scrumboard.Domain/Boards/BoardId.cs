using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Boards;

public readonly record struct BoardId(int Value) 
    : IStrongId<int, BoardId>
{
    public static implicit operator int(BoardId strongId) 
        => strongId.Value;

    public static explicit operator BoardId(int value) 
        => new(value);
}
