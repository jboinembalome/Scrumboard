using System.Collections.Generic;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.ValueObjects;

namespace Scrumboard.Domain.Cards;

public class Label : AuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Colour Colour { get; set; }
    public ICollection<Card> Cards { get; set; }
}