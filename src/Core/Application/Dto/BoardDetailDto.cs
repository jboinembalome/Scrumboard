using System;
using System.Collections.Generic;

namespace Scrumboard.Application.Dto
{
    public class BoardDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public Guid UserId { get; set; }
        public TeamDto Team { get; set; }
        public BoardSettingDto BoardSetting { get; set; }
        public IEnumerable<ListBoardDto> ListBoards { get; set; }
        public IEnumerable<LabelDto> Labels { get; set; }
    }
}
