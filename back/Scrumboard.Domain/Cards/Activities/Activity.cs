using Scrumboard.Domain.Common;

namespace Scrumboard.Domain.Cards.Activities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

public sealed class Activity
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

    public ActivityId Id { get; set; }
    public ActivityType ActivityType { get; set; }
    public ActivityField ActivityField { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public CardId CardId { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public UserId? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}
