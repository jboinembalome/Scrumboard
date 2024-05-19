#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Boards.Dtos;

public sealed class BoardDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Initials { get; set; }
    public string Uri { get; set; }
    public bool IsPinned { get; set; }
    public DateTime? LastActivity { get; set; }
    public int Members { get; set; }
    public BoardSettingDto BoardSetting { get; set; }
}
