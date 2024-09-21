using AutoFixture;
using FluentAssertions;
using Scrumboard.Application.Cards.Activities.ActivityStrategies;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards.Activities.ActivityStrategies;

public sealed class DueDateActivityStrategyTests
{
    private readonly IFixture _fixture = new CustomizedFixture();
    
    private readonly DueDateActivityStrategy _sut = new();

    [Fact]
    public async Task Should_return_empty_when_no_changes()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var dueDateChange = (OldValue: (DateTimeOffset?)null, NewValue: (DateTimeOffset?)null);
        
        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, dueDateChange, CancellationToken.None);
        
        // Assert
        activities.Should()
            .BeEmpty();
    }
    
    [Fact]
    public async Task Should_return_added_Activity_when_DueDate_has_been_added()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var dueDateChange = (OldValue: (DateTimeOffset?)null, NewValue: DateTimeOffset.Now);
        
        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, dueDateChange, CancellationToken.None);
        
        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId,
                ActivityType.Added,
                ActivityField.DueDate,
                string.Empty,
                dueDateChange.NewValue.Date.ToShortDateString())
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }
    
    [Fact]
    public async Task Should_return_removed_Activity_when_DueDate_has_been_removed()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var dueDateChange = (OldValue: DateTimeOffset.Now, NewValue: (DateTimeOffset?)null);
        
        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, dueDateChange, CancellationToken.None);
        
        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId,
                ActivityType.Removed,
                ActivityField.DueDate,
                dueDateChange.OldValue.Date.ToShortDateString(),
                string.Empty)
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }
    
    [Fact]
    public async Task Should_return_updated_Activity_when_DueDate_has_been_updated()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var dueDateChange = (OldValue: DateTimeOffset.Now.AddDays(-1), NewValue: DateTimeOffset.Now);
        
        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, dueDateChange, CancellationToken.None);
        
        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId,
                ActivityType.Updated,
                ActivityField.DueDate,
                dueDateChange.OldValue.Date.ToShortDateString(),
                dueDateChange.NewValue.Date.ToShortDateString())
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }
}
