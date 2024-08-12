using Scrumboard.Domain.Boards;
using Scrumboard.SharedKernel.Entities;
using Scrumboard.SharedKernel.Types;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Teams;

public sealed class Team : EntityBase<TeamId>, ICreatedAtEntity
{
    private readonly List<TeamMember> _teamMembers = [];
    
    public string Name { get; set; }
    public IReadOnlyCollection<TeamMember> Members => _teamMembers.ToList();
    public BoardId BoardId { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    public void AddMember(UserId memberId)
    {
        if (_teamMembers.Any(x => x.TeamId == Id && x.MemberId == memberId))
        {
            return;
        }
        
        _teamMembers.Add(new TeamMember { TeamId = Id, MemberId = memberId });
    }

    public void RemoveMember(UserId memberId)
    {
        var teamMember = _teamMembers.FirstOrDefault(x => x.TeamId == Id && x.MemberId == memberId);
        
        if (teamMember is null)
        {
            return;
        }
        
        _teamMembers.Remove(teamMember);
    }
}
