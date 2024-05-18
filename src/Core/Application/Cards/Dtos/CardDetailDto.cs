using System;
using System.Collections.Generic;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Boards.Dtos;

namespace Scrumboard.Application.Cards.Dtos;

public sealed class CardDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Suscribed { get; set; }
    public DateTime? DueDate { get; set; }
    public int Position { get; set; }
    public int ListBoardId { get; set; }
    public string ListBoardName { get; set; }
    public int BoardId { get; set; }
    public IEnumerable<LabelDto> Labels { get; set; }
    public IEnumerable<AdherentDto> Adherents { get; set; }
    public IEnumerable<AttachmentDto> Attachments { get; set; }
    public IEnumerable<ChecklistDto> Checklists { get; set; }
    public IEnumerable<CommentDto> Comments { get; set; }
    public IEnumerable<ActivityDto> Activities { get; set; }
}