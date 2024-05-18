using System.Collections.Generic;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Common;

namespace Scrumboard.Domain.Boards;

public sealed class Label : AuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Colour Colour { get; set; }
    public ICollection<Card> Cards { get; set; }
}