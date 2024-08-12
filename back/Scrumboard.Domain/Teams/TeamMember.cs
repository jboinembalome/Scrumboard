using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Teams;

public sealed class TeamMember
{
    public TeamId TeamId { get; set; }
    public UserId MemberId { get; set; }
}
