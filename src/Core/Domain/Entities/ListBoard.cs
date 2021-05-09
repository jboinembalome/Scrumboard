using Scrumboard.Domain.Common;
using System;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class ListBoard : AuditableEntity
    {
        public Guid ListBoardId { get; set; }
        public string Name { get; set; }
        public Board Board { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}
