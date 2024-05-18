using Scrumboard.Domain.Cards.Attachments;

namespace Scrumboard.Application.Cards.Dtos;

public class AttachmentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public AttachmentType AttachmentType { get; set; }
}