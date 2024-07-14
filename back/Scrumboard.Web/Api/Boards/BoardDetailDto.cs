using Scrumboard.Application.ListBoards.Dtos;
using Scrumboard.Application.Users.Dtos;
using TeamDto = Scrumboard.Web.Api.Teams.TeamDto;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Boards;

public sealed class BoardDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsPinned { get; set; }
    public TeamDto Team { get; set; }
    public BoardSettingDto BoardSetting { get; set; }
    public IEnumerable<ListBoardDto> ListBoards { get; set; }
    public UserDto CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserDto? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
