using System;
using Scrumboard.Application.Adherents.Dtos;

namespace Scrumboard.Application.Cards.Dtos;

public sealed class ActivityDto
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ActivityType { get; set; }
    public ActivityFieldDto ActivityField { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public AdherentDto Adherent { get; set; }
}