using Scrumboard.Domain.Boards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

public sealed class BoardEdition
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsPinned { get; set; }
    public BoardSetting BoardSetting { get; set; }
}
