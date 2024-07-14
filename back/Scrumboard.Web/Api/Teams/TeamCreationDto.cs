#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Teams;

public sealed class TeamCreationDto
{
    public string Name { get; set; } = string.Empty;
    public IReadOnlyCollection<string> MemberIds { get; set; }
}
