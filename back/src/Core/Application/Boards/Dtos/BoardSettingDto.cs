
using Scrumboard.Application.Common.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Boards.Dtos;

public sealed class BoardSettingDto
{
    public int Id { get; set; }
    public ColourDto Colour { get; set; }
    public bool Subscribed { get; set; }
    public bool CardCoverImage { get; set; }
}
