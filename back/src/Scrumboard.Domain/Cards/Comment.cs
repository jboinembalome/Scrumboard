using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards;

public sealed class Comment : IAuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Message { get; set; }
    public Adherent Adherent { get; set; }
    public Card Card { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
