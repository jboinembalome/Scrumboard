namespace Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

public abstract class ListBoardInputBase
{
    public string Name { get; set; }
    public int BoardId { get; set; }
    public int Position { get; set; }
}
