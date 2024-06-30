﻿using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Persistence.Adherents;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Persistence.Teams;

public sealed class TeamDao : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<AdherentDao> Adherents { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
