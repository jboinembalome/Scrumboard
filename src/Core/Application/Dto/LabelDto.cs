using System;

namespace Scrumboard.Application.Dto
{
    public class LabelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ColourDto Colour { get; set; }
    }
}
