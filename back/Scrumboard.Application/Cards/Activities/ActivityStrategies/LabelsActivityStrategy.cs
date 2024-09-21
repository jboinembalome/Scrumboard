using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.SharedKernel.Entities;

namespace Scrumboard.Application.Cards.Activities.ActivityStrategies;

internal sealed class LabelsActivityStrategy(
    ILabelsRepository labelsRepository) : IChangeActivityStrategy<IReadOnlyCollection<LabelId>>
{
    public async Task<IReadOnlyCollection<Activity>> GetChangeActivitiesAsync(
        CardId cardId, 
        PropertyValueChange<IReadOnlyCollection<LabelId>> labelIds, 
        CancellationToken cancellationToken)
    {
        var removedLabelIds = GetLabelChanges(labelIds.NewValue, labelIds.OldValue);
        var addedLabelIds = GetLabelChanges(labelIds.OldValue, labelIds.NewValue);
    
        var labelIdsToFetch = GetLabelIdsToFetch([..removedLabelIds, ..addedLabelIds]);
        if (labelIdsToFetch.Length <= 0)
        {
            return [];
        }

        var activities = new List<Activity>();
        
        var labels = (await labelsRepository.GetAsync(labelIdsToFetch, cancellationToken))
            .ToDictionary(label => label.Id);
    
        var removedLabels = GetLabels(removedLabelIds, labels);
        if (removedLabels.Length > 0)
        {
            activities.Add(new Activity(
                cardId,
                ActivityType.Removed,
                ActivityField.Label,
                string.Join(", ", removedLabels.Select(x => x.Name)),
                string.Empty));
        }
        
        var addedLabels = GetLabels(addedLabelIds, labels);
        if (addedLabels.Length > 0)
        {
            activities.Add(new Activity(
                cardId,
                ActivityType.Added,
                ActivityField.Label,
                string.Empty,
                string.Join(", ", addedLabels.Select(x => x.Name))));
        }

        return activities;
    }
    
    private static LabelId[] GetLabelChanges(
        IReadOnlyCollection<LabelId> labelIds1, 
        IReadOnlyCollection<LabelId> labelIds2)
        => labelIds2.Except(labelIds1).ToArray();

    private static LabelId[] GetLabelIdsToFetch(IEnumerable<LabelId> labelIds)
        => labelIds.ToArray();

    private static Label[] GetLabels(
        IEnumerable<LabelId> labelIds, 
        IReadOnlyDictionary<LabelId, Label> labelsDetails)
        => labelIds
            .Select(labelsDetails.GetValueOrDefault)
            .Where(x => x is not null)
            .ToArray()!;
}
