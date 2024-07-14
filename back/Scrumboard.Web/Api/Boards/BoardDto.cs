using Scrumboard.Web.Api.Users;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Boards;

public sealed class BoardDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsPinned { get; set; }
    public BoardSettingDto BoardSetting { get; set; }
    public UserDto CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserDto? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
