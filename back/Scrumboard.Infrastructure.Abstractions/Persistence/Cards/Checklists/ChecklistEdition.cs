#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Checklists;

public sealed class ChecklistEdition
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IReadOnlyCollection<ChecklistItemEdition> ChecklistItems { get; set; }
}
