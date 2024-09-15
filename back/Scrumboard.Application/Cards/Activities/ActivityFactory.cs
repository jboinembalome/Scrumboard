using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Events;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Application.Cards.Activities;

internal sealed class ActivityFactory(
    ILabelsRepository labelsRepository,
    IIdentityService identityService) : IActivityFactory
{
    public async Task<IReadOnlyCollection<Activity>> CreateAsync(
        CardUpdatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var activities = new List<Activity>();

        activities.AddRange(GetNameChangeActivities(domainEvent));
        activities.AddRange(GetDescriptionChangeActivities(domainEvent));
        activities.AddRange(GetDueDateChangeActivities(domainEvent));
        activities.AddRange(await GetAssigneeChangeActivitiesAsync(domainEvent, cancellationToken));
        activities.AddRange(await GetLabelChangeActivitiesAsync(domainEvent, cancellationToken));

        return activities;
    }

    private static IEnumerable<Activity> GetNameChangeActivities(CardUpdatedDomainEvent domainEvent)
    {
        if (domainEvent.Name.OldValue != domainEvent.Name.NewValue)
        {
            yield return new Activity(
                domainEvent.Id,
                ActivityType.Updated,
                ActivityField.Name,
                domainEvent.Name.OldValue,
                domainEvent.Name.NewValue);
        }
    }

    private static IEnumerable<Activity> GetDescriptionChangeActivities(CardUpdatedDomainEvent domainEvent)
    {
        if (domainEvent.Description.OldValue != domainEvent.Description.NewValue)
        {
            yield return new Activity(
                domainEvent.Id,
                ActivityType.Updated,
                ActivityField.Description,
                domainEvent.Description.OldValue ?? string.Empty,
                domainEvent.Description.NewValue ?? string.Empty);
        }
    }

    private static IEnumerable<Activity> GetDueDateChangeActivities(CardUpdatedDomainEvent domainEvent)
    {
        if (domainEvent.DueDate.OldValue != domainEvent.DueDate.NewValue)
        {
            if (!domainEvent.DueDate.OldValue.HasValue && domainEvent.DueDate.NewValue.HasValue)
            {
                yield return new Activity(
                    domainEvent.Id,
                    ActivityType.Added,
                    ActivityField.DueDate,
                    string.Empty,
                    domainEvent.DueDate.NewValue.Value.Date.ToShortDateString());
            }

            if (domainEvent.DueDate.OldValue.HasValue && !domainEvent.DueDate.NewValue.HasValue)
            {
                yield return new Activity(
                    domainEvent.Id,
                    ActivityType.Removed,
                    ActivityField.DueDate,
                    domainEvent.DueDate.OldValue.Value.Date.ToShortDateString(),
                    string.Empty);
            }

            if (domainEvent.DueDate.OldValue.HasValue
                && domainEvent.DueDate.NewValue.HasValue
                && domainEvent.DueDate.OldValue.Value != domainEvent.DueDate.NewValue.Value)
            {
                yield return new Activity(
                    domainEvent.Id,
                    ActivityType.Updated,
                    ActivityField.DueDate,
                    domainEvent.DueDate.OldValue.Value.Date.ToShortDateString(),
                    domainEvent.DueDate.NewValue.Value.Date.ToShortDateString());
            }
        }
    }

    private async Task<IReadOnlyCollection<Activity>> GetAssigneeChangeActivitiesAsync(
        CardUpdatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var activities = new List<Activity>();

        var oldAssignees = domainEvent.AssigneeIds.OldValue.ToHashSet();
        var newAssignees = domainEvent.AssigneeIds.NewValue.ToHashSet();

        var addedAssignees = newAssignees.Except(oldAssignees).ToArray();
        var removedAssignees = oldAssignees.Except(newAssignees).ToArray();

        var assigneeIdsToFetch = addedAssignees
            .Concat(removedAssignees)
            .Select(id => (UserId)id.Value)
            .ToArray();

        if (assigneeIdsToFetch.Length > 0)
        {
            var assigneesDetails = await identityService.GetListAsync(assigneeIdsToFetch, cancellationToken);

            var assigneesDetailsDict = assigneesDetails.ToDictionary(assignee => assignee.Id);

            foreach (var assigneeId in addedAssignees)
            {
                if (assigneesDetailsDict.TryGetValue(assigneeId.Value, out var assignee))
                {
                    activities.Add(new Activity(
                        domainEvent.Id,
                        ActivityType.Added,
                        ActivityField.Member,
                        string.Empty,
                        $"{assignee.FirstName} {assignee.LastName}"));
                }
            }

            foreach (var assigneeId in removedAssignees)
            {
                if (assigneesDetailsDict.TryGetValue(assigneeId.Value, out var assignee))
                {
                    activities.Add(new Activity(
                        domainEvent.Id,
                        ActivityType.Removed,
                        ActivityField.Member,
                        $"{assignee.FirstName} {assignee.LastName}",
                        string.Empty));
                }
            }
        }

        return activities;
    }

    private async Task<IReadOnlyCollection<Activity>> GetLabelChangeActivitiesAsync(
        CardUpdatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var activities = new List<Activity>();

        var oldLabels = domainEvent.LabelIds.OldValue.ToHashSet();
        var newLabels = domainEvent.LabelIds.NewValue.ToHashSet();

        var addedLabels = newLabels.Except(oldLabels).ToArray();
        var removedLabels = oldLabels.Except(newLabels).ToArray();

        var labelIdsToFetch = addedLabels.Concat(removedLabels).ToArray();

        if (labelIdsToFetch.Length > 0)
        {
            var labelsDetails = await labelsRepository.GetAsync(labelIdsToFetch, cancellationToken);

            var labelsDetailsDict = labelsDetails.ToDictionary(label => label.Id);

            foreach (var labelId in addedLabels)
            {
                if (labelsDetailsDict.TryGetValue(labelId, out var label))
                {
                    activities.Add(new Activity(
                        domainEvent.Id,
                        ActivityType.Added,
                        ActivityField.Label,
                        string.Empty,
                        label.Name));
                }
            }

            foreach (var labelId in removedLabels)
            {
                if (labelsDetailsDict.TryGetValue(labelId, out var label))
                {
                    activities.Add(new Activity(
                        domainEvent.Id,
                        ActivityType.Removed,
                        ActivityField.Label,
                        label.Name,
                        string.Empty));
                }
            }
        }

        return activities;
    }
}
