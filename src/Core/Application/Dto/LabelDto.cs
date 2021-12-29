using System.Collections.Generic;

namespace Scrumboard.Application.Dto
{
    public class LabelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ColourDto Colour { get; set; }
        public IEnumerable<int> CardIds { get; set; }
    }
}
