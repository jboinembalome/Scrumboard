using AutoFixture;
using FluentAssertions;
using Moq;
using Scrumboard.Application.Cards.Activities.ActivityStrategies;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.SharedKernel.Types;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards.Activities.ActivityStrategies;

public sealed class AssigneesActivityStrategyTests
{
    private readonly IFixture _fixture;
    
    private readonly IIdentityService _identityService;
    
    private readonly AssigneesActivityStrategy _sut;

    public AssigneesActivityStrategyTests()
    {
        _fixture = new CustomizedFixture();
        
        _identityService = Mock.Of<IIdentityService>();
        
        _sut = new AssigneesActivityStrategy(_identityService);
    }
    
    [Fact]
    public async Task Should_return_empty_when_no_assignees_changes()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var assigneeIds = (
            OldValue: new AssigneeId[] { new("xxx") }, 
            NewValue: new AssigneeId[] { new("xxx") });
        
        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, assigneeIds, CancellationToken.None);
        
        // Assert
        activities.Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Should_return_added_Activity_when_assignees_have_been_added()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var assigneeIds = (
            OldValue: Array.Empty<AssigneeId>(), 
            NewValue: new AssigneeId[] { new("xxx") });
        
        var addedAssignees = Given_assignees(assigneeIds.NewValue);

        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, assigneeIds, CancellationToken.None);

        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId,
                ActivityType.Added,
                ActivityField.Assignees,
                string.Empty,
                string.Join(", ", addedAssignees.Select(x => $"{x.FirstName} {x.LastName}")))
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }

    [Fact]
    public async Task Should_return_removed_Activity_when_assignees_have_been_removed()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var assigneeIds = (
            OldValue: new AssigneeId[] { new("xxx") }, 
            NewValue: Array.Empty<AssigneeId>());
        
        var removedAssignees = Given_assignees(assigneeIds.OldValue);

        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, assigneeIds, CancellationToken.None);

        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId,
                ActivityType.Updated,
                ActivityField.Assignees,
                string.Join(", ", removedAssignees.Select(x => $"{x.FirstName} {x.LastName}")),
                string.Empty)
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }

    [Fact]
    public async Task Should_return_removed_and_added_Activity_when_assignees_have_been_removed_and_added()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var assigneeIds = (
            OldValue: new AssigneeId[] { new("xxx") }, 
            NewValue: new AssigneeId[] { new("yyy") });
        
        var assignees = Given_assignees([..assigneeIds.OldValue, ..assigneeIds.NewValue]);

        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, assigneeIds, CancellationToken.None);

        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId,
                ActivityType.Updated,
                ActivityField.Assignees,
                $"{assignees[0].FirstName} {assignees[0].LastName}",
                string.Empty),
            new(
                cardId,
                ActivityType.Added,
                ActivityField.Assignees,
                string.Empty,
                $"{assignees[1].FirstName} {assignees[1].LastName}"),
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }
    
    private IUser[] Given_assignees(IReadOnlyCollection<AssigneeId> assigneeIds)
    {
        var userIds = assigneeIds
            .Select(assigneeId => (UserId)assigneeId.Value)
            .ToList();

        var users = userIds
            .Select(userId =>
            {
                var user = Mock.Of<IUser>();
                user.Id = userId.Value;
                user.FirstName = _fixture.Create<string>();
                user.LastName = _fixture.Create<string>();
                user.Job = _fixture.Create<string>();
                user.Avatar = _fixture.Create<byte[]>();

                return user;
            })
            .ToArray();

        Mock.Get(_identityService)
            .Setup(x => x.GetListAsync(userIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        return users;
    }
}

