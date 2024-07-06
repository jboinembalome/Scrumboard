#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Persistence.Cards;

public sealed class CardAssigneeDao
{
    public int CardId { get; set; }
    public string AssigneeId { get; set; }
}
