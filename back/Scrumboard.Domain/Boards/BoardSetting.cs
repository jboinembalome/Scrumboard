using Scrumboard.Domain.Common;
using Scrumboard.SharedKernel.Entities;

namespace Scrumboard.Domain.Boards;

public sealed class BoardSetting : EntityBase<BoardSettingId>
{
    public BoardId BoardId { get; set; }
    public Colour Colour { get; set; } = Colour.Gray;
}
