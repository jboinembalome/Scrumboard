using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards;

public sealed class CardAssignee
{
    public CardId CardId { get; set; }
    public UserId AssigneeId { get; set; }
}
