using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Persistence.Cards.Activities;

public sealed class ActivityDao : IAuditableEntity
{
    public int Id { get; set; }
    public ActivityType ActivityType { get; set; }
    public ActivityField ActivityField { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public int CardId { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
