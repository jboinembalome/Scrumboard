using Scrumboard.Domain.Common;
using Scrumboard.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class Board : AuditableEntity, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public Guid UserId { get; set; }
        public Team Team { get; set; }
        public BoardSetting BoardSetting { get; set; }
        public ICollection<ListBoard> ListBoards { get; set; }
        public ICollection<Label> Labels { get; set; }
    }
}
