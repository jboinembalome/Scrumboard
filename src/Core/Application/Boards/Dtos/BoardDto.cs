using System;

namespace Scrumboard.Application.Boards.Dtos;

public class BoardDto
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