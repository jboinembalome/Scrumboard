using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.ListBoards;
using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards;

public sealed class Card : AuditableEntityBase<CardId>
{
    private readonly List<CardAssignee> _assignees = [];
    private readonly List<CardLabel> _labels = [];
    
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    public int Position { get; set; }
    public ListBoardId ListBoardId { get; set; }
    public IReadOnlyCollection<CardAssignee> Assignees => _assignees.AsReadOnly();
    public IReadOnlyCollection<CardLabel> Labels => _labels.AsReadOnly();
    
    public void UpdateAssignees(IEnumerable<AssigneeId> assigneeIds)
    {
        var assigneeIdsList = assigneeIds.ToHashSet();
        
        if (_assignees.Count == assigneeIdsList.Count 
            && _assignees.All(x => assigneeIdsList.Contains(x.AssigneeId)))
        {
            return;
        }

        RemoveAssigneesNoLongerPresent(assigneeIdsList);
        AddNewAssignees(assigneeIdsList);
    }
    
    public void UpdateLabels(IEnumerable<LabelId> labelIds)
    {
        var labelIdsList = labelIds.ToHashSet();
        
        if (_labels.Count == labelIdsList.Count 
            && _labels.All(x => labelIdsList.Contains(x.LabelId)))
        {
            return;
        }

        RemoveLabelsNoLongerPresent(labelIdsList);
        AddNewLabels(labelIdsList);
    }

    private void RemoveAssigneesNoLongerPresent(IEnumerable<AssigneeId> assigneeIds)
    {
        var assigneesToRemove = _assignees
            .Where(x => !assigneeIds.Contains(x.AssigneeId))
            .ToArray();

        foreach (var assignee in assigneesToRemove)
        {
            _assignees.Remove(assignee);
        }
    }

    private void AddNewAssignees(IEnumerable<AssigneeId> assigneeIds)
    {
        var assigneesToAdd = assigneeIds
            .Where(assigneeId => !_assignees.Exists(x => x.AssigneeId == assigneeId))
            .Select(assigneeId => new CardAssignee { CardId = Id, AssigneeId = assigneeId })
            .ToArray();

        foreach (var assignee in assigneesToAdd)
        {
            _assignees.Add(assignee);
        }
    }

    private void RemoveLabelsNoLongerPresent(IEnumerable<LabelId> labelIds)
    {
        var labelsToRemove = _labels
            .Where(x => !labelIds.Contains(x.LabelId))
            .ToArray();

        foreach (var label in labelsToRemove)
        {
            _labels.Remove(label);
        }
    }

    private void AddNewLabels(IEnumerable<LabelId> labelIds)
    {
        var labelsToAdd = labelIds
            .Where(labelId => !_labels.Exists(x => x.LabelId == labelId))
            .Select(labelId => new CardLabel { CardId = Id, LabelId = labelId })
            .ToArray();

        foreach (var label in labelsToAdd)
        {
            _labels.Add(label);
        }
    }
}
