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

        // Remove assignees who are no longer in the list
        _assignees.RemoveAll(x => !assigneeIdsList.Contains(x.AssigneeId));
        
        AddAssignees(assigneeIdsList);
    }
    
    public void UpdateLabels(IEnumerable<LabelId> labelIds)
    {
        var labelIdsList = labelIds.ToHashSet();
        
        if (_labels.Count == labelIdsList.Count 
            && _labels.All(x => labelIdsList.Contains(x.LabelId)))
        {
            return;
        }

        // Remove labels who are no longer in the list
        _labels.RemoveAll(x => !labelIdsList.Contains(x.LabelId));
        
        AddLabels(labelIdsList);
    }
    
    private void AddAssignees(IEnumerable<AssigneeId> assigneeIds)
    {
        var assigneeIdsList = assigneeIds.ToHashSet();
        
        var assigneesToAdd = assigneeIdsList
            .Where(assigneeId => !_assignees.Exists(x => x.AssigneeId == assigneeId))
            .Select(assigneeId => new CardAssignee { CardId = Id, AssigneeId = assigneeId })
            .ToArray();
        
        // Add new assignees only if there are any
        if (assigneesToAdd.Length > 0)
        {
            _assignees.AddRange(assigneesToAdd);
        }
    }
    
    private void AddLabels(IEnumerable<LabelId> labelIds)
    {
        var labelIdsList = labelIds.ToHashSet();
        
        var labelsToAdd = labelIdsList
            .Where(labelId => !_labels.Exists(x => x.LabelId == labelId))
            .Select(labelId => new CardLabel { CardId = Id, LabelId = labelId })
            .ToArray();
        
        // Add new labels only if there are any
        if (labelsToAdd.Length > 0)
        {
            _labels.AddRange(labelsToAdd);
        }
    }

}
