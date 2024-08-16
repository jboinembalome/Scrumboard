using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards;

public sealed class Card : AuditableEntityBase<CardId>
{
    private readonly List<CardAssignee> _cardAssignees = [];
    private readonly List<CardLabel> _cardLabels = [];
    
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    public int Position { get; set; }
    public ListBoardId ListBoardId { get; set; }
    public IReadOnlyCollection<CardAssignee> Assignees => _cardAssignees.ToList();
    public IReadOnlyCollection<CardLabel> Labels => _cardLabels.ToList();
    
    public void AddAssignee(AssigneeId assigneeId)
    {
        if (_cardAssignees.Any(x => x.CardId == Id && x.AssigneeId == assigneeId))
        {
            return;
        }
        
        _cardAssignees.Add(new CardAssignee { CardId = Id, AssigneeId = assigneeId });
    }

    public void RemoveAssignee(AssigneeId assigneeId)
    {
        var cardAssignee = _cardAssignees.FirstOrDefault(x => x.CardId == Id && x.AssigneeId == assigneeId);
        
        if (cardAssignee is null)
        {
            return;
        }
        
        _cardAssignees.Remove(cardAssignee);
    }
    
    public void AddLabel(LabelId labelId)
    {
        if (_cardLabels.Any(x => x.CardId == Id && x.LabelId == labelId))
        {
            return;
        }
        
        _cardLabels.Add(new CardLabel { CardId = Id, LabelId = labelId });
    }

    public void RemoveLabel(LabelId labelId)
    {
        var cardLabel = _cardLabels.FirstOrDefault(x => x.CardId == Id && x.LabelId == labelId);
        
        if (cardLabel is null)
        {
            return;
        }
        
        _cardLabels.Remove(cardLabel);
    }
}
