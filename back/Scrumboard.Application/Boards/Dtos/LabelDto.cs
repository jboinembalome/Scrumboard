using Scrumboard.Application.Common.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Boards.Dtos;

public sealed class LabelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ColourDto Colour { get; set; }
    public IEnumerable<int> CardIds { get; set; }
}
