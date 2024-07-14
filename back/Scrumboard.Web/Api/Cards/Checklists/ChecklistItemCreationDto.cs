#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Cards.Checklists;

public sealed class ChecklistItemCreationDto
{
    public string Name { get; set; }
    public bool IsChecked { get; set; }
}
