using Scrumboard.SharedKernel.Entities;

namespace Scrumboard.Domain.Cards.Activities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

public sealed class Activity : AuditableEntityBase<ActivityId>
{
    public Activity()
    {
    }

    public Activity(
        CardId cardId, 
        ActivityType activityType, 
        ActivityField activityField, 
        string? oldValue, 
        string? newValue)
    {
        ActivityType = activityType;
        ActivityField = activityField;
        OldValue = oldValue;
        NewValue = newValue;
        CardId = cardId;
    }
    
    public ActivityType ActivityType { get; set; }
    public ActivityField ActivityField { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public CardId CardId { get; set; }
}
