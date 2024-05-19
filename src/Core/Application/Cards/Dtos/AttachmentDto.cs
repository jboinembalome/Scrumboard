using Scrumboard.Domain.Cards.Attachments;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Cards.Dtos;

public sealed class AttachmentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public AttachmentType AttachmentType { get; set; }
}
