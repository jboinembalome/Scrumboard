using Scrumboard.Domain.Common;
using System;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class Team : AuditableEntity
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
        public ICollection<Guid> UserIds { get; set; }
    }
}
