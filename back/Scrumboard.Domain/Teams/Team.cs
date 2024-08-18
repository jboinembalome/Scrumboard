using Scrumboard.Domain.Boards;
using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Teams;

public sealed class Team : CreatedAtEntityBase<TeamId>
{
    private readonly List<TeamMember> _teamMembers = [];
    
    public string Name { get; set; }
    public IReadOnlyCollection<TeamMember> Members => _teamMembers.AsReadOnly();
    public BoardId BoardId { get; set; }
    
    public void AddMember(MemberId memberId)
    {
        if (_teamMembers.Any(x => x.TeamId == Id && x.MemberId == memberId))
        {
            return;
        }
        
        _teamMembers.Add(new TeamMember { TeamId = Id, MemberId = memberId });
    }

    public void RemoveMember(MemberId memberId)
    {
        var teamMember = _teamMembers.FirstOrDefault(x => x.TeamId == Id && x.MemberId == memberId);
        
        if (teamMember is null)
        {
            return;
        }
        
        _teamMembers.Remove(teamMember);
    }
}
