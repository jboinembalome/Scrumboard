﻿using System.Collections.Generic;

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
        public IEnumerable<ListBoardDto> ListBoards { get; set; }
    }
}
