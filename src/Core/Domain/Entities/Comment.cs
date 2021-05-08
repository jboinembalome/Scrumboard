using Scrumboard.Domain.Common;
using System;

namespace Scrumboard.Domain.Entities
{
    public class Comment : AuditableEntity
    {
        public Guid CommentId { get; set; }
        public string Message { get; set; }
        public Guid UserId { get; set; }
        public Card Card { get; set; }
    }
}
