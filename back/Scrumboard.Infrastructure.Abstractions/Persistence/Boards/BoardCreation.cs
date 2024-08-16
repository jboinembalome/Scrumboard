using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Boards;

public sealed class BoardCreation
{
    public string Name { get; set; }
    public bool IsPinned { get; set; }
    public BoardSettingCreation BoardSetting { get; set; }
    public TeamCreation Team { get; set; }
    public OwnerId OwnerId { get; set; }
}
