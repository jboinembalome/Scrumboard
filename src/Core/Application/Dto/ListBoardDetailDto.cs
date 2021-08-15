using System.Collections.Generic;

namespace Scrumboard.Application.Dto
{
    public class ListBoardDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public IEnumerable<CardDto> Cards { get; set; }
    }
}
