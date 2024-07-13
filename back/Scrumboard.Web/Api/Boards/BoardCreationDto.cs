
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Boards;

public sealed class BoardCreationDto
{
    public string Name { get; set; }
    public bool IsPinned { get; set; }
    public BoardSettingCreationDto BoardSetting { get; set; }
}
