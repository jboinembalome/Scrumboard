#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Abstractions.Cards.Checklists;

public sealed class ChecklistItemEdition
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsChecked { get; set; }
}
