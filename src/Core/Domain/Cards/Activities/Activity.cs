using System;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Domain.Cards.Activities;

public sealed class Activity : IAuditableEntity, IEntity<int>
{
    public Activity()
    {

    }

    public Activity(ActivityType activityType, ActivityField activityField, string oldValue, string newValue)
    {
        ActivityType = activityType;
        ActivityField = activityField;
        OldValue = oldValue;
        NewValue = newValue;
    }

    public int Id { get; set; }
    public ActivityType ActivityType { get; set; }
    public ActivityField ActivityField { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public Adherent Adherent { get; set; }
    public Card Card { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}