using Scrumboard.Domain.ListBoards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

public sealed class ListBoardEdition : ListBoardInputBase
{
    public ListBoardId Id { get; set; }
}
