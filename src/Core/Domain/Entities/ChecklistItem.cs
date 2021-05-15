using Scrumboard.Domain.Common;
using Scrumboard.Domain.Interfaces;
using System;

namespace Scrumboard.Domain.Entities
{
    public class ChecklistItem : AuditableEntity, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public Checklist Checklist { get; set; }
    }
}
