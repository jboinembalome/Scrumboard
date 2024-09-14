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
        IReadOnlyCollection<MemberId> memberIds)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        Name = name;
        BoardId = boardId;

        if (memberIds.Count > 0)
        {
            AddNewMembers(memberIds);
        }  
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

        RemoveMembersNoLongerPresent(memberIdsList);
        AddNewMembers(memberIdsList);
    }

    private void RemoveMembersNoLongerPresent(IEnumerable<MemberId> memberIds)
    {
        var membersToRemove = _members
            .Where(x => !memberIds.Contains(x.MemberId))
            .ToArray();

        foreach (var member in membersToRemove)
        {
            _members.Remove(member);
        }
    }

    private void AddNewMembers(IEnumerable<MemberId> memberIds)
    {   
        var membersToAdd = memberIds
            .Where(memberId => !_members.Exists(x => x.MemberId == memberId))
            .Select(memberId => new TeamMember { TeamId = Id, MemberId = memberId })
            .ToArray();
        
        foreach (var member in membersToAdd)
        {
            _members.Add(member);
        }
    }
}
