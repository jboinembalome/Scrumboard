using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.ListBoards.Dtos;
using Scrumboard.Application.Teams.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Boards.Dtos;

public sealed class BoardDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public AdherentDto Adherent { get; set; }
    public TeamDto Team { get; set; }
    public BoardSettingDto BoardSetting { get; set; }
    public IEnumerable<ListBoardDto> ListBoards { get; set; }
}
