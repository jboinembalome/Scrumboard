using Scrumboard.Domain.Boards;
using Scrumboard.SharedKernel.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Teams;

public sealed class Team : CreatedAtEntityBase<TeamId>
{
    private readonly List<TeamMember> _members = [];

    public Team() { }
    
    public Team(
        string name, 
        BoardId boardId,
        IEnumerable<MemberId> memberIds)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        Name = name;
        BoardId = boardId;
        
        AddMembers(memberIds);
    }

    public string Name { get; private set; }
    public BoardId BoardId { get; private set; }
    public IReadOnlyCollection<TeamMember> Members => _members.AsReadOnly();
    
    public void UpdateMembers(IEnumerable<MemberId> memberIds)
    {
        var memberIdsList = memberIds.ToHashSet();
        
        if (_members.Count == memberIdsList.Count 
            && _members.All(x => memberIdsList.Contains(x.MemberId)))
        {
            return;
        }

        // Remove members who are no longer in the list
        _members.RemoveAll(x => !memberIdsList.Contains(x.MemberId));
        
        AddMembers(memberIdsList);
    }
    
    private void AddMembers(IEnumerable<MemberId> memberIds)
    {
        var memberIdsList = memberIds.ToHashSet();
        
        var membersToAdd = memberIdsList
            .Where(memberId => !_members.Exists(tm => tm.MemberId == memberId))
            .Select(memberId => new TeamMember { TeamId = Id, MemberId = memberId })
            .ToArray();
        
        // Add new members only if there are any
        if (membersToAdd.Length > 0)
        {
            _members.AddRange(membersToAdd);
        }
    }
}
