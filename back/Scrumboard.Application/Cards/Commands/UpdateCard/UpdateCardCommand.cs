using MediatR;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Application.Cards.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Cards.Commands.UpdateCard;

public sealed class UpdateCardCommand : IRequest<UpdateCardCommandResponse>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool Suscribed { get; set; }
    public DateTime? DueDate { get; set; }
    public IEnumerable<LabelDto> Labels { get; set; }
    public IEnumerable<AdherentDto> Adherents { get; set; }
    public IEnumerable<AttachmentDto> Attachments { get; set; } = [];
    public IEnumerable<ChecklistDto> Checklists { get; set; }
}
