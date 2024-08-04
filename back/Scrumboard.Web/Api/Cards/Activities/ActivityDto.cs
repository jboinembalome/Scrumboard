using Scrumboard.Web.Api.Users;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Cards.Activities;

public sealed class ActivityDto
{
    public int Id { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string ActivityType { get; set; }
    public ActivityFieldDto ActivityField { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public UserDto User { get; set; }
}
