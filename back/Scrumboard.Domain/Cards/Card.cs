using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Checklists;
using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards;

public sealed class Card : IAuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool Suscribed { get; set; }
    public DateTime? DueDate { get; set; }
    public int Position { get; set; }
    public int ListBoardId { get; set; }
    public ICollection<Label> Labels { get; set; }
    public ICollection<Activity> Activities { get; set; }
    public ICollection<Checklist> Checklists { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Guid> Assignees { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
