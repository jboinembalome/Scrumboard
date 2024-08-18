using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.ListBoards;

public sealed class ListBoardId(
    int value) : IntStrongId<ListBoardId>(value), IStrongId<ListBoardId, int>
{
    public static explicit operator ListBoardId(int value)
        => new(value);
}
