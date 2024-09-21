using AutoFixture;
using FluentAssertions;
using Moq;
using Scrumboard.Application.Cards.Activities.ActivityStrategies;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards.Activities.ActivityStrategies;

public sealed class LabelsActivityStrategyTests
{
    private readonly IFixture _fixture;
    
    private readonly ILabelsRepository _labelsRepository;
    
    private readonly LabelsActivityStrategy _sut;

    public LabelsActivityStrategyTests()
    {
        _fixture = new CustomizedFixture();
        
        _labelsRepository = Mock.Of<ILabelsRepository>();
        
        _sut = new LabelsActivityStrategy(_labelsRepository);
    }
    
    [Fact]
    public async Task Should_return_empty_when_no_label_changes()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var labelIds = (
            OldValue: new LabelId[] { new(1) }, 
            NewValue: new LabelId[] { new(1) });
        
        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, labelIds, CancellationToken.None);
        
        // Assert
        activities.Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Should_return_added_Activity_when_labels_have_been_added()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var labelIds = (
            OldValue: Array.Empty<LabelId>(), 
            NewValue: new LabelId[] { new(1) });
        
        var addedLabels = Given_labels(labelIds.NewValue);

        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, labelIds, CancellationToken.None);

        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId,
                ActivityType.Added,
                ActivityField.Label,
                string.Empty,
                string.Join(", ", addedLabels.Select(x => x.Name)))
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }

    [Fact]
    public async Task Should_return_removed_Activity_when_labels_have_been_removed()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var labelIds = (
            OldValue: new LabelId[] { new(1) }, 
            NewValue: Array.Empty<LabelId>());
        
        var removedLabels = Given_labels(labelIds.OldValue);

        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, labelIds, CancellationToken.None);

        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId,
                ActivityType.Removed,
                ActivityField.Label,
                string.Join(", ", removedLabels.Select(x => x.Name)),
                string.Empty)
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }

    [Fact]
    public async Task Should_return_removed_and_added_Activity_when_labels_have_been_removed_and_added()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var labelIds = (
            OldValue: new LabelId[] { new(1) }, 
            NewValue: new LabelId[] { new(2) });
        
        var labels = Given_labels([..labelIds.OldValue, ..labelIds.NewValue]);

        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, labelIds, CancellationToken.None);

        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId,
                ActivityType.Removed,
                ActivityField.Label,
                $"{labels[0].Name}",
                string.Empty),
            new(
                cardId,
                ActivityType.Added,
                ActivityField.Label,
                string.Empty,
                $"{labels[1].Name}"),
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }
    
    private Label[] Given_labels(IReadOnlyCollection<LabelId> labelIds)
    {
        var labels = labelIds
            .Select(id =>
            {
                var label = _fixture.Create<Label>();
                label.SetProperty(x => x.Id, id);
                label.SetProperty(x => x.Name, _fixture.Create<string>());
                
                return label;
            })
            .ToArray();

        Mock.Get(_labelsRepository)
            .Setup(x => x.GetAsync(labelIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(labels);

        return labels;
    }
}

