using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Boards.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Cards.Dtos;

public sealed class CardDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool Suscribed { get; set; }
    public DateTime? DueDate { get; set; }
    public int Position { get; set; }
    public int ListBoardId { get; set; }
    public IEnumerable<LabelDto> Labels { get; set; }
    public IEnumerable<AdherentDto> Assignees { get; set; }
    public IEnumerable<ChecklistDto> Checklists { get; set; }
}
