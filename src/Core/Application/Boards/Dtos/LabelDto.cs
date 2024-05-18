using System.Collections.Generic;
using Scrumboard.Application.Common.Dtos;

namespace Scrumboard.Application.Boards.Dtos;

public class LabelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ColourDto Colour { get; set; }
    public IEnumerable<int> CardIds { get; set; }
}