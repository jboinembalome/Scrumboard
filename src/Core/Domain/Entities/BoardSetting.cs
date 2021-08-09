using Scrumboard.Domain.Interfaces;
using Scrumboard.Domain.ValueObjects;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class BoardSetting: IEntity<int>
    {
        public int Id { get; set; }
        public Colour Colour { get; set; } = Colour.Gray;
        public bool Subscribed { get; set; } = false;
        public bool CardCoverImage { get; set; } = false;
        public ICollection<Board> Boards { get; set; }
    }
}
