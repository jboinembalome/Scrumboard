using Scrumboard.Domain.Interfaces;
using Scrumboard.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class BoardSetting: IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Colour Colour { get; set; } = Colour.White;
        public bool Subscribed { get; set; } = false;
        public bool CardCoverImage { get; set; } = false;
        public ICollection<Board> Boards { get; set; }
    }
}
