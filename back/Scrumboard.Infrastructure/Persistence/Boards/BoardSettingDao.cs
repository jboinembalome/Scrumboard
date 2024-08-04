using Scrumboard.Domain.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Persistence.Boards;

public sealed class BoardSettingDao
{
    public int Id { get; set; }
    public Colour Colour { get; set; } = Colour.Gray;
    public int BoardId { get; set; }
}
