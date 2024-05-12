using System;
using System.Collections.Generic;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.Entities;
using Scrumboard.Domain.ListBoards;

namespace Scrumboard.Domain.Boards;

public class Board : AuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = "Untitled Board";
    public string Uri { get; set; } = "untitled-board";
    public bool IsPinned { get; set; } = false;
    public Adherent Adherent { get; set; }
    public Team Team { get; set; }
    public BoardSetting BoardSetting { get; set; }
    public ICollection<ListBoard> ListBoards { get; set; }
    
    public string GetInitials()
    {
        if (string.IsNullOrEmpty(Name))
            return Name;

        var nameSplit = Name
            .Trim()
            .Split(new[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
        
        var initials = string.Empty;

        foreach (var item in nameSplit)
            initials += item.Substring(0, 1);

        return initials.ToUpper();
    }
}