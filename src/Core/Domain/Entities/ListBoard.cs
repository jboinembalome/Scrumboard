using Scrumboard.Domain.Common;
using Scrumboard.Domain.Interfaces;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class ListBoard : AuditableEntity, IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Board Board { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}
