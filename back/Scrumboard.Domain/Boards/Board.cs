using Scrumboard.Domain.Common;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Domain.Teams;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Boards;

public class Board : IAuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = "Untitled Board";
    public string Uri { get; set; } = "untitled-board";
    public bool IsPinned { get; set; }
    public Team Team { get; set; }
    public BoardSetting BoardSetting { get; set; }
    public ICollection<ListBoard> ListBoards { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    
    public string GetInitials()
    {
        if (string.IsNullOrEmpty(Name))
            return Name;

        var nameSplit = Name
            .Trim()
            .Split([",", " "], StringSplitOptions.RemoveEmptyEntries);
        
        var initials = string.Empty;

        foreach (var item in nameSplit)
            initials += item.Substring(0, 1);

        return initials.ToUpper();
    }
}
