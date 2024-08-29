using Scrumboard.Domain.Boards;
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
    
    public void AddAssignee(AssigneeId assigneeId)
    {
        if (_assignees.Any(x => x.CardId == Id && x.AssigneeId == assigneeId))
        {
            return;
        }
        
        _assignees.Add(new CardAssignee { CardId = Id, AssigneeId = assigneeId });
    }

    public void RemoveAssignee(AssigneeId assigneeId)
    {
        var cardAssignee = _assignees.FirstOrDefault(x => x.CardId == Id && x.AssigneeId == assigneeId);
        
        if (cardAssignee is null)
        {
            return;
        }
        
        _assignees.Remove(cardAssignee);
    }
    
    public void AddLabel(LabelId labelId)
    {
        if (_labels.Any(x => x.CardId == Id && x.LabelId == labelId))
        {
            return;
        }
        
        _labels.Add(new CardLabel { CardId = Id, LabelId = labelId });
    }

    public void RemoveLabel(LabelId labelId)
    {
        var cardLabel = _labels.FirstOrDefault(x => x.CardId == Id && x.LabelId == labelId);
        
        if (cardLabel is null)
        {
            return;
        }
        
        _labels.Remove(cardLabel);
    }
}
