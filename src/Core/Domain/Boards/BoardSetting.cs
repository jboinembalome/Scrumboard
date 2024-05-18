using System.Collections.Generic;
using Scrumboard.Domain.Common;

namespace Scrumboard.Domain.Boards;

public class BoardSetting: IEntity<int>
{
    public int Id { get; set; }
    public Colour Colour { get; set; } = Colour.Gray;
    public bool Subscribed { get; set; } = false;
    public bool CardCoverImage { get; set; } = false;
    public ICollection<Board> Boards { get; set; }
}