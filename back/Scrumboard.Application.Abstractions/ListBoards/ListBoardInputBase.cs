using Scrumboard.Domain.Boards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Abstractions.ListBoards;

public abstract class ListBoardInputBase
{
    public string Name { get; set; }
    public BoardId BoardId { get; set; }
    public int Position { get; set; }
}
