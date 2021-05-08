using Scrumboard.Domain.Common;
using Scrumboard.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class Label : AuditableEntity
    {
        public Guid LabelId { get; set; }
        public string Name { get; set; }
        public CustomColor CustomColor { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}
