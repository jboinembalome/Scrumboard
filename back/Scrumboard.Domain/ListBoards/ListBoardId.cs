using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.ListBoards;

public readonly record struct ListBoardId(int Value)
    : IStrongId<int, ListBoardId>
{
    public static implicit operator int(ListBoardId strongId)
        => strongId.Value;

    public static explicit operator ListBoardId(int value)
        => new(value);
}
