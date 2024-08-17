using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Infrastructure.Abstractions.Persistence.Teams;

public sealed class TeamEdition
{
    public TeamId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IReadOnlyCollection<MemberId> MemberIds { get; set; }
    public BoardId BoardId { get; set; }
}
