using System.Collections.Generic;
using Scrumboard.Application.Adherents.Dtos;
using Scrumboard.Application.ListBoards.Dtos;
using Scrumboard.Application.Teams.Dtos;

namespace Scrumboard.Application.Boards.Dtos;

public class BoardDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public AdherentDto Adherent { get; set; }
    public TeamDto Team { get; set; }
    public BoardSettingDto BoardSetting { get; set; }
    public IEnumerable<ListBoardDto> ListBoards { get; set; }
}