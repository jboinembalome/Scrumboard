using Scrumboard.Domain.Common;
using System;

namespace Scrumboard.Domain.Entities
{
    public class ListBoard : AuditableEntity
    {
        public Guid ListBoardId { get; set; }
        public string Name { get; set; }
        public Board Board { get; set; }
    }
}
