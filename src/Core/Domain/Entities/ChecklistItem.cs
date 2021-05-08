using Scrumboard.Domain.Common;
using System;

namespace Scrumboard.Domain.Entities
{
    public class ChecklistItem : AuditableEntity
    {
        public Guid ChecklistItemId { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public Checklist Checklist { get; set; }
    }
}
