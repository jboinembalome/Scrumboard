using System.Collections.Generic;

namespace Scrumboard.Application.Dto
{
    public class BoardDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public AdherentDto Adherent { get; set; }
        public TeamDto Team { get; set; }
        public BoardSettingDto BoardSetting { get; set; }
        public IEnumerable<ListBoardDetailDto> ListBoards { get; set; }
        public IEnumerable<LabelDto> Labels { get; set; }
    }
}
