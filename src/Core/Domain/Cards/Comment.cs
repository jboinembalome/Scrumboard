using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Domain.Cards;

public class Comment : AuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Message { get; set; }
    public Adherent Adherent { get; set; }
    public Card Card { get; set; }
}