using Scrumboard.Application.Users.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Cards.Dtos;

public sealed class ActivityDto
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ActivityType { get; set; }
    public ActivityFieldDto ActivityField { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public UserDto User { get; set; }
}
