using Scrumboard.Domain.Common;
using Scrumboard.Domain.Interfaces;

namespace Scrumboard.Domain.Entities
{
    public class ChecklistItem : AuditableEntity, IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public Checklist Checklist { get; set; }
    }
}
