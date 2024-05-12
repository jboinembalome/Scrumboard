using System;
using System.Collections.Generic;
using MediatR;
using Scrumboard.Application.Dto;

namespace Scrumboard.Application.Cards.UpdateCard;

public class UpdateCardCommand : IRequest<UpdateCardCommandResponse>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Suscribed { get; set; }
    public DateTime? DueDate { get; set; }
    public IEnumerable<LabelDto> Labels { get; set; }
    public IEnumerable<AdherentDto> Adherents { get; set; }
    public IEnumerable<AttachmentDto> Attachments { get; set; }
    public IEnumerable<ChecklistDto> Checklists { get; set; }
}