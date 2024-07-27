using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Persistence.ListBoards;
using Scrumboard.Infrastructure.Persistence.Teams;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Persistence.Boards;

public sealed class BoardDao : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public bool IsPinned { get; set; }
    public TeamDao Team { get; set; }
    public BoardSettingDao BoardSetting { get; set; }
    public ICollection<ListBoardDao> ListBoards { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
