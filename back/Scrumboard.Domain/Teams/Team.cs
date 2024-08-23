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
    
    public void UpdateMembers(IEnumerable<MemberId> memberIds)
    {
        var memberIdsList = memberIds.ToList();
        
        if (_teamMembers.Count == memberIdsList.Count 
            && _teamMembers.All(x => memberIdsList.Contains(x.MemberId)))
        {
            return;
        }

        // Remove members who are no longer in the list
        _teamMembers.RemoveAll(x => !memberIdsList.Contains(x.MemberId));
        
        var membersToAdd = memberIdsList
            .Where(memberId => !_teamMembers.Exists(tm => tm.MemberId == memberId))
            .Select(memberId => new TeamMember { TeamId = Id, MemberId = memberId })
            .ToArray();
        
        // Add new members only if there are any
        if (membersToAdd.Length > 0)
        {
            _teamMembers.AddRange(membersToAdd);
        }
    }
}
