using AutoFixture;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Domain.UnitTests.Teams;

public sealed class TeamTests
{
    private readonly IFixture _fixture = new CustomizedFixture();
    
    [Fact]
    public void UpdateMembers_should_remove_Members_who_are_no_longer_in_the_list()
    {
        // Arrange
        var memberIds = _fixture.CreateMany<MemberId>(3).ToArray();
        
        var team = Given_a_Team(memberIds);

        var updatedMemberIds = memberIds[2..];

        // Act
        team.UpdateMembers(updatedMemberIds);

        // Assert
        var expectedMembers = updatedMemberIds
            .Select(memberId => new TeamMember
            {
                TeamId = team.Id, 
                MemberId = memberId
            })
            .ToArray();
        
        team.Members.Should()
            .ContainSingle()
            .And.BeEquivalentTo(expectedMembers);
    }
    
    [Fact]
    public void UpdateMembers_should_add_new_Members()
    {
        // Arrange
        var memberIds = _fixture.CreateMany<MemberId>(3).ToArray();
        
        var team = Given_a_Team(memberIds);

        var newMemberIds = _fixture.CreateMany<MemberId>(2).ToArray();

        MemberId[] updatedMemberIds = [..memberIds, ..newMemberIds];
        
        // Act
        team.UpdateMembers(updatedMemberIds);

        // Assert
        var expectedMembers = updatedMemberIds
            .Select(memberId => new TeamMember
            {
                TeamId = team.Id, 
                MemberId = memberId
            })
            .ToArray();
        
        team.Members.Should()
            .HaveCount(5)
            .And.BeEquivalentTo(expectedMembers);
    }

    [Fact]
    public void UpdateMembers_should_remove_all_Members_when_list_is_empty()
    {
        // Arrange
        var memberIds = _fixture.CreateMany<MemberId>(3).ToArray();
        
        var team = Given_a_Team(memberIds);

        // Act
        team.UpdateMembers([]);

        // Assert
        team.Members.Should()
            .BeEmpty();
    }

    [Fact]
    public void UpdateMembers_should_do_nothing_when_Members_are_already_up_to_date()
    {
        // Arrange
        var memberIds = _fixture.CreateMany<MemberId>(3).ToList();
        
        var team = Given_a_Team(memberIds);

        // Act
        team.UpdateMembers(memberIds);

        // Assert
        var expectedMembers = memberIds
            .Select(memberId => new TeamMember
            {
                TeamId = team.Id, 
                MemberId = memberId
            })
            .ToArray();
        
        team.Members.Should()
            .HaveCount(3)
            .And.BeEquivalentTo(expectedMembers);
    }
    
    private Team Given_a_Team(IReadOnlyCollection<MemberId> memberIds) 
        => new(
            name: _fixture.Create<string>(),
            boardId: _fixture.Create<BoardId>(),
            memberIds: memberIds);
}
