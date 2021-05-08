using Scrumboard.Domain.Common;
using System;

namespace Scrumboard.Domain.Entities
{
    public class Board : AuditableEntity
    {
        public Guid BoardId { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public Guid UserId { get; set; }
    }
}
