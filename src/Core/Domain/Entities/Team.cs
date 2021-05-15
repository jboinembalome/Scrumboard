using Scrumboard.Domain.Common;
using Scrumboard.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class Team : AuditableEntity, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Guid> UserIds { get; set; }
        public ICollection<Board> Boards { get; set; }
    }
}
