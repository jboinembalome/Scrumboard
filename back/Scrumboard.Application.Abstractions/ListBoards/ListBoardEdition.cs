using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Abstractions.ListBoards;

public sealed class ListBoardEdition
{
    public ListBoardId Id { get; set; }
    public string Name { get; set; }
    public BoardId BoardId { get; set; }
    public int Position { get; set; }
}
