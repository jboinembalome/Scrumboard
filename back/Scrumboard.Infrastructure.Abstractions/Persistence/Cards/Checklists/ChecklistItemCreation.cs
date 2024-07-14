#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Checklists;

public sealed class ChecklistItemCreation
{
    public string Name { get; set; }
    public bool IsChecked { get; set; }
}
