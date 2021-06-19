using Scrumboard.Domain.Interfaces;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class Adherent: IEntity<int>
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public ICollection<Board> Boards { get; set; }
        public ICollection<Card> Cards { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
