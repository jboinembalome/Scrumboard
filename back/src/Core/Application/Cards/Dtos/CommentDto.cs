using Scrumboard.Application.Adherents.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Cards.Dtos;

public sealed class CommentDto
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public AdherentDto Adherent { get; set; }
}
