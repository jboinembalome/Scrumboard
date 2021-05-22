using Scrumboard.Domain.Common;
using Scrumboard.Domain.Interfaces;

namespace Scrumboard.Domain.Entities
{
    public class Activity : AuditableEntity, IEntity<int>
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public Adherent Adherent { get; set; }
        public Card Card { get; set; }
    }
}
