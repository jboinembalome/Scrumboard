using System.Collections.Generic;

namespace Scrumboard.Application.Dto
{
    public class ListBoardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<CardDto> Cards { get; set; }
    }
}
