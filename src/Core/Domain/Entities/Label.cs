using Scrumboard.Domain.Common;
using Scrumboard.Domain.Interfaces;
using Scrumboard.Domain.ValueObjects;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class Label : AuditableEntity, IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Colour Colour { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}
