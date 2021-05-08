using Scrumboard.Domain.Common;
using System;

namespace Scrumboard.Domain.Entities
{
    public class Activity : AuditableEntity
    {
        public Guid ActivityId { get; set; }
        public string Message { get; set; }
        public Guid UserId { get; set; }
        public Card Card { get; set; }
    }
}
