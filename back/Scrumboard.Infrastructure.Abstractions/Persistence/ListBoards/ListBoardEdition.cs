using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

public sealed class ListBoardEdition : ListBoardInputBase
{
    public ListBoardId Id { get; set; }
}
