namespace Scrumboard.Infrastructure.Abstractions.Persistence.Boards.ListBoards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

public sealed class ListBoardEdition
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }
    //public IEnumerable<CardEdition> Cards { get; set; }
}
