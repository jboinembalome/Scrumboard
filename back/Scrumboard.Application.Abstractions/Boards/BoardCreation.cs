using Scrumboard.Domain.Boards;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Application.Abstractions.Boards;

public sealed class BoardCreation
{
    public string Name { get; set; }
    public bool IsPinned { get; set; }
    public BoardSettingCreation BoardSetting { get; set; }
    public OwnerId OwnerId { get; set; }
}
