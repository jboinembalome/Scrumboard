using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.SharedKernel.Entities;

namespace Scrumboard.Application.Cards.Activities.ActivityStrategies;

internal sealed class DueDateActivityStrategy : IChangeActivityStrategy<DateTimeOffset?>
{
    public Task<IReadOnlyCollection<Activity>> GetChangeActivitiesAsync(
        CardId cardId, 
        PropertyValueChange<DateTimeOffset?> dueDate, 
        CancellationToken cancellationToken)
    {
        var activities = GetDueDateChangeActivities(cardId, dueDate);
        
        return Task.FromResult(activities);
    }
    
    private static IReadOnlyCollection<Activity> GetDueDateChangeActivities(
        CardId cardId, 
        PropertyValueChange<DateTimeOffset?> dueDate)
    {
        if (dueDate.OldValue == dueDate.NewValue)
        {
            return [];
        }

        return dueDate switch
        {
            { OldValue: null, NewValue: not null } => 
            [
                new Activity(
                    cardId,
                    ActivityType.Added,
                    ActivityField.DueDate,
                    string.Empty,
                    dueDate.NewValue.Value.Date.ToShortDateString())
            ],
            { OldValue: not null, NewValue: null } =>
            [
                new Activity(
                    cardId,
                    ActivityType.Removed,
                    ActivityField.DueDate,
                    dueDate.OldValue.Value.Date.ToShortDateString(),
                    string.Empty)
            ],
            { OldValue: not null, NewValue: not null } 
                when dueDate.OldValue.Value != dueDate.NewValue.Value =>
            [
                new Activity(
                    cardId,
                    ActivityType.Updated,
                    ActivityField.DueDate,
                    dueDate.OldValue.Value.Date.ToShortDateString(),
                    dueDate.NewValue.Value.Date.ToShortDateString())
            ],
            _ => []
        };
    }
}
