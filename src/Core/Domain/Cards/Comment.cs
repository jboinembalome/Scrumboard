using System;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Common;

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