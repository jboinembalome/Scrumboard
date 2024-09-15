using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.ListBoards;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.Domain.Cards.Events;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards;

public sealed class Card : AuditableEntityBase<CardId>
{
    private readonly List<CardAssignee> _assignees = [];
    private readonly List<CardLabel> _labels = [];

    public Card() { }

    public Card(
        string name, 
        string? description, 
        DateTimeOffset? dueDate, 
        int position, 
        ListBoardId listBoardId,
        IReadOnlyCollection<AssigneeId> assigneeIds,
        IReadOnlyCollection<LabelId> labelIds)
    {
        Name = name;
        Description = description;
        DueDate = dueDate;
        Position = position;
        ListBoardId = listBoardId;

        if (assigneeIds.Count > 0)
        {
            AddNewAssignees(assigneeIds);
        }

        if (labelIds.Count > 0)
        {
            AddNewLabels(labelIds);
        }    
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset? DueDate { get; private set; }
    public int Position { get; private set; }
    public ListBoardId ListBoardId { get; private set; }
    public IReadOnlyCollection<CardAssignee> Assignees => _assignees.AsReadOnly();
    public IReadOnlyCollection<CardLabel> Labels => _labels.AsReadOnly();

    public void Update(
        string name,
        string? description,
        DateTimeOffset? dueDate,
        int position,
        ListBoardId listBoardId,
        IReadOnlyCollection<AssigneeId> assigneeIds,
        IReadOnlyCollection<LabelId> labelIds)
    {
        if (Id is null)
        {
            throw new InvalidOperationException($"Cannot update a {nameof(Card)} when {nameof(Id)} is null.");
        }

        Name = name; 
        Description = description;
        DueDate = dueDate;
        Position = position;
        ListBoardId = listBoardId;
        
        UpdateAssignees(assigneeIds);
        UpdateLabels(labelIds);

        AddDomainEvent(new CardUpdatedDomainEvent(Id));
    }

    private void UpdateAssignees(IEnumerable<AssigneeId> assigneeIds)
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
    
    private void UpdateLabels(IEnumerable<LabelId> labelIds)
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
