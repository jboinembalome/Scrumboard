using System.Collections.Generic;
using Scrumboard.Domain.Common;
using Scrumboard.Domain.Entities;
using Scrumboard.Domain.Interfaces;

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
}