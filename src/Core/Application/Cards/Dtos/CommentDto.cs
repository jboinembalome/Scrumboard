using System;
using Scrumboard.Application.Adherents.Dtos;

namespace Scrumboard.Application.Cards.Dtos;

public class CommentDto
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public AdherentDto Adherent { get; set; }
}