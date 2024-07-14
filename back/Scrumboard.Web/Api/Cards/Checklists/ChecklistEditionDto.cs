#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Cards.Checklists;

public sealed class ChecklistEditionDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IReadOnlyCollection<ChecklistItemEditionDto> ChecklistItems { get; set; }
}
