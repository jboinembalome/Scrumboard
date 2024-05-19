using System;
using System.Collections.Generic;
using Scrumboard.Domain.Adherents;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;

namespace Scrumboard.Domain.Teams;

public sealed class Team : IAuditableEntity, IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Adherent> Adherents { get; set; }
    public ICollection<Board> Boards { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}