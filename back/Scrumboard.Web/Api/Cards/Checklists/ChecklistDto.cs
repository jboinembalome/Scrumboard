#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Cards.Checklists;

public sealed class ChecklistDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IReadOnlyCollection<ChecklistItemDto> ChecklistItems { get; set; }
}
