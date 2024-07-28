using Scrumboard.Web.Api.Cards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Boards.ListBoards;

public sealed class ListBoardWithCardDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }
    public IEnumerable<CardDto> Cards { get; set; }
}
