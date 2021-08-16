using System;
using System.Collections.Generic;

namespace Scrumboard.Application.Dto
{
    public class CardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Suscribed { get; set; }
        public DateTime? DueDate { get; set; }
        public int Position { get; set; }
        public int ListBoardId { get; set; }
        public IEnumerable<LabelDto> Labels { get; set; }
        public IEnumerable<AdherentDto> Adherents { get; set; }
        public int AttachmentsCount { get; set; }
        public int ChecklistItemsCount { get; set; }
        public int ChecklistItemsDoneCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
