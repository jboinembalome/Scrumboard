using Scrumboard.Domain.Common;

namespace Scrumboard.Domain.Boards;

public sealed class BoardSetting
{
    public BoardSettingId Id { get; set; }
    public BoardId BoardId { get; set; }
    public Colour Colour { get; set; } = Colour.Gray;
}
