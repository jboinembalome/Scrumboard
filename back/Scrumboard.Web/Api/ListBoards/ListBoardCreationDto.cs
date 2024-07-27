
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.ListBoards;

public sealed class ListBoardCreationDto
{
    public string Name { get; set; }
    public int BoardId { get; set; }
}
