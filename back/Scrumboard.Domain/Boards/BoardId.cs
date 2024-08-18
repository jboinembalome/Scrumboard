using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Boards;

public sealed class BoardId(
    int value) : IntStrongId<BoardId>(value), IStrongId<BoardId, int>
{
    public static explicit operator BoardId(int value)
        => new(value);
}
