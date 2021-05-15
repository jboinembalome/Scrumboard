using Scrumboard.Domain.Common;
using Scrumboard.Domain.Interfaces;
using System;

namespace Scrumboard.Domain.Entities
{
    public class Comment : AuditableEntity, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public Guid UserId { get; set; }
        public Card Card { get; set; }
    }
}
