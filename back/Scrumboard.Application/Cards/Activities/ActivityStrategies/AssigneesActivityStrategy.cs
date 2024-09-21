using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Application.Cards.Activities.ActivityStrategies;

internal sealed class AssigneesActivityStrategy(
    IIdentityService identityService) : IChangeActivityStrategy<IReadOnlyCollection<AssigneeId>>
{
    public async Task<IReadOnlyCollection<Activity>> GetChangeActivitiesAsync(
        CardId cardId, 
        PropertyValueChange<IReadOnlyCollection<AssigneeId>> assigneeIds, 
        CancellationToken cancellationToken)
    {
        var removedAssigneeIds = GetAssigneeChanges(assigneeIds.NewValue, assigneeIds.OldValue);
        var addedAssigneeIds = GetAssigneeChanges(assigneeIds.OldValue, assigneeIds.NewValue);
        
        var assigneeIdsToFetch = GetAssigneeIdsToFetch([..removedAssigneeIds, ..addedAssigneeIds]);
        if (assigneeIdsToFetch.Length <= 0)
        {
            return [];
        }

        var activities = new List<Activity>();
        
        var assignees = (await identityService.GetListAsync(assigneeIdsToFetch, cancellationToken))
            .ToDictionary(assignee => assignee.Id);
        
        var removedAssignees = GetAssignees(removedAssigneeIds, assignees);
        if (removedAssignees.Length > 0)
        {
            activities.Add(new Activity(
                cardId,
                ActivityType.Updated,
                ActivityField.Assignees,
                string.Join(", ", removedAssignees.Select(x => $"{x.FirstName} {x.LastName}")),
                string.Empty));
        }
        
        var addedAssignees = GetAssignees(addedAssigneeIds, assignees);
        if (addedAssignees.Length > 0)
        {
            activities.Add(new Activity(
                cardId,
                ActivityType.Added,
                ActivityField.Assignees,
                string.Empty,
                string.Join(", ", addedAssignees.Select(x => $"{x.FirstName} {x.LastName}"))));
        }
        
        return activities;
    }
    
    private static AssigneeId[] GetAssigneeChanges(
        IReadOnlyCollection<AssigneeId> assigneeIds1, 
        IReadOnlyCollection<AssigneeId> assigneeIds2) 
        => assigneeIds2.Except(assigneeIds1).ToArray();
    
    private static UserId[] GetAssigneeIdsToFetch(IEnumerable<AssigneeId> assigneeIds) 
        => assigneeIds
            .Select(id => (UserId)id.Value)
            .ToArray();

    private static IUser[] GetAssignees(
        IEnumerable<AssigneeId> assigneeIds, 
        IReadOnlyDictionary<string, IUser> assigneesDetails) 
        => assigneeIds
            .Select(id => assigneesDetails.GetValueOrDefault(id.Value))
            .Where(x => x is not null)
            .ToArray()!;
}
