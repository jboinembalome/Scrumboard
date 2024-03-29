﻿using Scrumboard.Domain.Common;
using Scrumboard.Domain.Interfaces;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class Team : AuditableEntity, IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Adherent> Adherents { get; set; }
        public ICollection<Board> Boards { get; set; }
    }
}
