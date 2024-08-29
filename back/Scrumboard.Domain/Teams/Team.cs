using Scrumboard.Domain.Boards;
using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Teams;

public sealed class Team : CreatedAtEntityBase<TeamId>
{
    private readonly List<TeamMember> _teamMembers = [];

    public Team() { }
    
    public Team(string name, BoardId boardId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        Name = name;
        BoardId = boardId;
    }

    public string Name { get; private set; }
    public BoardId BoardId { get; private set; }
    public IReadOnlyCollection<TeamMember> Members => _teamMembers.AsReadOnly();
    
    public void AddMembers(IEnumerable<MemberId> memberIds)
    {
        var memberIdsList = memberIds.ToHashSet();
        
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
    
    public void UpdateMembers(IEnumerable<MemberId> memberIds)
    {
        var memberIdsList = memberIds.ToHashSet();
        
        if (_teamMembers.Count == memberIdsList.Count 
            && _teamMembers.All(x => memberIdsList.Contains(x.MemberId)))
        {
            return;
        }

        // Remove members who are no longer in the list
        _teamMembers.RemoveAll(x => !memberIdsList.Contains(x.MemberId));
        
        AddMembers(memberIdsList);
    }
}
