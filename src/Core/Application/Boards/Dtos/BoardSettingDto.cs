
using Scrumboard.Application.Common.Dtos;

namespace Scrumboard.Application.Boards.Dtos;

public class BoardSettingDto
{
    public int Id { get; set; }
    public ColourDto Colour { get; set; }
    public bool Subscribed { get; set; }
    public bool CardCoverImage { get; set; }
}