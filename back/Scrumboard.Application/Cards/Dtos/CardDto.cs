using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.Users.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Cards.Dtos;

public sealed class CardDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Suscribed { get; set; }
    public DateTime? DueDate { get; set; }
    public int Position { get; set; }
    public int ListBoardId { get; set; }
    public IEnumerable<LabelDto> Labels { get; set; }
    public IEnumerable<UserDto> Assignees { get; set; }
    public int AttachmentsCount { get; set; }
    public int ChecklistItemsCount { get; set; }
    public int ChecklistItemsDoneCount { get; set; }
}
