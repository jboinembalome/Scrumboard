#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Web.Api.Boards;

public sealed class LabelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ColourDto Colour { get; set; }
    public int BoardId { get; set; }
}
