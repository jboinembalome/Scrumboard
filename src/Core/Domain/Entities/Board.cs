using Scrumboard.Domain.Common;
using Scrumboard.Domain.Interfaces;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class Board : AuditableEntity, IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Untitled Board";
        public string Uri { get; set; } = "untitled-board";
        public Adherent Adherent { get; set; }
        public Team Team { get; set; }
        public BoardSetting BoardSetting { get; set; } = new BoardSetting();
        public ICollection<ListBoard> ListBoards { get; set; }
        public ICollection<Label> Labels { get; set; }
    }
}
