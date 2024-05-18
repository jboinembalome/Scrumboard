using Scrumboard.Domain.Common;
using System.Collections.Generic;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Domain.Entities;

public class Team : AuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Adherent> Adherents { get; set; }
    public ICollection<Board> Boards { get; set; }
}