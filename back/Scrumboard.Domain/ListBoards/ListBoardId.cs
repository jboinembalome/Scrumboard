using System.Diagnostics.CodeAnalysis;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.ListBoards;

public sealed class ListBoardId(
    int value) : IntStrongId<ListBoardId>(value), IStrongId<ListBoardId, int>
{
    [return: NotNullIfNotNull(nameof(id))]
    public static implicit operator ListBoardId?(int? id)
        => id is null ? null : new ListBoardId(id.Value);
}
