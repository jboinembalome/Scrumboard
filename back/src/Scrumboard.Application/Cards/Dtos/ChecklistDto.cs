#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Cards.Dtos;

public sealed class ChecklistDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ChecklistItemDto> ChecklistItems { get; set; }
}
