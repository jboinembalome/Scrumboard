using Scrumboard.Domain.Common;
using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Boards;

public sealed class BoardSetting : EntityBase<BoardSettingId>
{
    public BoardSetting() { }

    public BoardSetting(Colour colour)
    {
        Colour = colour;
    }
    
    public BoardId BoardId { get; private set; }
    public Colour Colour { get; private set; } = Colour.Gray;
}
