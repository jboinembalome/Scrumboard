using FluentAssertions;
using Moq;
using Scrumboard.Application.Cards.Activities;
using Scrumboard.Application.Cards.Activities.ActivityStrategies;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Events;
using Scrumboard.Domain.ListBoards;
using Scrumboard.SharedKernel.Entities;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards.Activities;

public sealed class ActivityFactoryTests
{
    private readonly IChangeActivityStrategy<DateTimeOffset?> _dueDateChangeActivityStrategy;
    private readonly IChangeActivityStrategy<ListBoardId> _listBoardChangeActivityStrategy;
    private readonly IChangeActivityStrategy<IReadOnlyCollection<AssigneeId>> _assigneesChangeActivityStrategy;
    private readonly IChangeActivityStrategy<IReadOnlyCollection<LabelId>> _labelsChangeActivityStrategy;

    private readonly ActivityFactory _sut;

    public ActivityFactoryTests()
    {
        _dueDateChangeActivityStrategy = Mock.Of<IChangeActivityStrategy<DateTimeOffset?>>();
        _listBoardChangeActivityStrategy = Mock.Of<IChangeActivityStrategy<ListBoardId>>();
        _assigneesChangeActivityStrategy = Mock.Of<IChangeActivityStrategy<IReadOnlyCollection<AssigneeId>>>();
        _labelsChangeActivityStrategy = Mock.Of<IChangeActivityStrategy<IReadOnlyCollection<LabelId>>>();

        _sut = new ActivityFactory(
            _dueDateChangeActivityStrategy,
            _listBoardChangeActivityStrategy,
            _assigneesChangeActivityStrategy,
            _labelsChangeActivityStrategy);
        
        InitializeMocks();
    }

    [Fact]
    public async Task Factory_should_create_activity_when_DueDate_has_been_added()
    {
        // Arrange
        var domainEvent = new CardUpdatedDomainEvent(
            id: new CardId(1),
            name: (OldValue: "Name", NewValue: "Name"),
            description: (OldValue: "Description", NewValue: "Description"),
            dueDate: (OldValue: null, NewValue: DateTimeOffset.Now),
            position: (OldValue: 1, NewValue: 1),
            listBoardId: (OldValue: 1, NewValue: 1),
            assigneeIds: (OldValue: [new AssigneeId("xxx")], NewValue: [new AssigneeId("xxx")]),
            labelIds: (OldValue: [new LabelId(1)], NewValue: [new LabelId(1)]));

        Given_DueDate_activities(
            domainEvent.Id,
            domainEvent.DueDate,
            [
                new Activity(
                    cardId: domainEvent.Id,
                    activityType: ActivityType.Added,
                    activityField: ActivityField.DueDate,
                    oldValue: string.Empty,
                    newValue: domainEvent.DueDate.NewValue!.Value.Date.ToShortDateString())
            ]);

        // Act
        var activities = await _sut.CreateAsync(domainEvent, CancellationToken.None);

        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId: domainEvent.Id,
                activityType: ActivityType.Added,
                activityField: ActivityField.DueDate,
                oldValue: string.Empty,
                newValue: domainEvent.DueDate.NewValue.Value.Date.ToShortDateString())
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }
    
    [Fact]
    public async Task Factory_should_create_activity_when_ListBoardId_has_been_updated()
    {
        // Arrange
        var domainEvent = new CardUpdatedDomainEvent(
            id: new CardId(1),
            name: (OldValue: "Name", NewValue: "Name"),
            description: (OldValue: "Description", NewValue: "Description"),
            dueDate: (OldValue: null, NewValue: null),
            position: (OldValue: 1, NewValue: 1),
            listBoardId: (OldValue: 1, NewValue: 2),
            assigneeIds: (OldValue: [], NewValue: []),
            labelIds: (OldValue: [], NewValue: []));

        Given_ListBoard_activities(
            domainEvent.Id,
            domainEvent.ListBoardId,
            [
                new Activity(
                    cardId: domainEvent.Id,
                    activityType: ActivityType.Updated,
                    activityField: ActivityField.ListBoard,
                    oldValue: "Backlog",
                    newValue: "Todo")
            ]);

        // Act
        var activities = await _sut.CreateAsync(domainEvent, CancellationToken.None);

        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId: domainEvent.Id,
                activityType: ActivityType.Updated,
                activityField: ActivityField.ListBoard,
                oldValue: "Backlog",
                newValue: "Todo")
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }
    
    [Fact]
    public async Task Factory_should_create_activity_when_AssigneeIds_has_been_updated()
    {
        // Arrange
        var domainEvent = new CardUpdatedDomainEvent(
            id: new CardId(1),
            name: (OldValue: "Name", NewValue: "Name"),
            description: (OldValue: "Description", NewValue: "Description"),
            dueDate: (OldValue: null, NewValue: null),
            position: (OldValue: 1, NewValue: 1),
            listBoardId: (OldValue: 1, NewValue: 1),
            assigneeIds: (OldValue: [], NewValue: [new AssigneeId("xxx")]),
            labelIds: (OldValue: [], NewValue: []));

        Given_ListBoard_activities(
            domainEvent.Id,
            domainEvent.ListBoardId,
            [
                new Activity(
                    cardId: domainEvent.Id,
                    activityType: ActivityType.Updated,
                    activityField: ActivityField.Assignees,
                    oldValue: string.Empty,
                    newValue: "John Doe")
            ]);

        // Act
        var activities = await _sut.CreateAsync(domainEvent, CancellationToken.None);

        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId: domainEvent.Id,
                activityType: ActivityType.Updated,
                activityField: ActivityField.Assignees,
                oldValue: string.Empty,
                newValue: "John Doe")
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }
    
    [Fact]
    public async Task Factory_should_create_activity_when_LabelIds_has_been_updated()
    {
        // Arrange
        var domainEvent = new CardUpdatedDomainEvent(
            id: new CardId(1),
            name: (OldValue: "Name", NewValue: "Name"),
            description: (OldValue: "Description", NewValue: "Description"),
            dueDate: (OldValue: null, NewValue: null),
            position: (OldValue: 1, NewValue: 1),
            listBoardId: (OldValue: 1, NewValue: 1),
            assigneeIds: (OldValue: [], NewValue: []),
            labelIds: (OldValue: [], NewValue: [new LabelId(1)]));

        Given_ListBoard_activities(
            domainEvent.Id,
            domainEvent.ListBoardId,
            [
                new Activity(
                    cardId: domainEvent.Id,
                    activityType: ActivityType.Updated,
                    activityField: ActivityField.Label,
                    oldValue: string.Empty,
                    newValue: "Back")
            ]);

        // Act
        var activities = await _sut.CreateAsync(domainEvent, CancellationToken.None);

        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId: domainEvent.Id,
                activityType: ActivityType.Updated,
                activityField: ActivityField.Label,
                oldValue: string.Empty,
                newValue: "Back")
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }

    [Fact]
    public async Task Factory_should_return_empty_when_no_changes()
    {
        // Arrange
        var domainEvent = new CardUpdatedDomainEvent(
            id: new CardId(1),
            name: (OldValue: "Name", NewValue: "Name"),
            description: (OldValue: "Description", NewValue: "Description"),
            dueDate: (OldValue: null, NewValue: null),
            position: (OldValue: 1, NewValue: 1),
            listBoardId: (OldValue: 1, NewValue: 1),
            assigneeIds: (OldValue: [], NewValue: []),
            labelIds: (OldValue: [], NewValue: []));

        Given_DueDate_activities(domainEvent.Id, domainEvent.DueDate, []);
        Given_ListBoard_activities(domainEvent.Id, domainEvent.ListBoardId, []);
        Given_Assignees_activities(domainEvent.Id, domainEvent.AssigneeIds, []);
        Given_Labels_activities(domainEvent.Id, domainEvent.LabelIds, []);

        // Act
        var activities = await _sut.CreateAsync(domainEvent, CancellationToken.None);

        // Assert
        activities.Should()
            .BeEmpty();
    }

    private void InitializeMocks()
    {
        Mock.Get(_dueDateChangeActivityStrategy)
            .Setup(x => x.GetChangeActivitiesAsync(
                It.IsAny<CardId>(), 
                It.IsAny<PropertyValueChange<DateTimeOffset?>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        Mock.Get(_listBoardChangeActivityStrategy)
            .Setup(x => x.GetChangeActivitiesAsync(
                It.IsAny<CardId>(), 
                It.IsAny<PropertyValueChange<ListBoardId>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        Mock.Get(_assigneesChangeActivityStrategy)
            .Setup(x => x.GetChangeActivitiesAsync(
                It.IsAny<CardId>(), 
                It.IsAny<PropertyValueChange<IReadOnlyCollection<AssigneeId>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        Mock.Get(_labelsChangeActivityStrategy)
            .Setup(x => x.GetChangeActivitiesAsync(
                It.IsAny<CardId>(), 
                It.IsAny<PropertyValueChange<IReadOnlyCollection<LabelId>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
    }
    
    private void Given_DueDate_activities(
        CardId cardId,
        PropertyValueChange<DateTimeOffset?> dueDate,
        IReadOnlyCollection<Activity> activities)
        => Mock.Get(_dueDateChangeActivityStrategy)
            .Setup(x => x.GetChangeActivitiesAsync(cardId, dueDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activities);

    private void Given_ListBoard_activities(
        CardId cardId,
        PropertyValueChange<ListBoardId> listBoardId,
        IReadOnlyCollection<Activity> activities)
        => Mock.Get(_listBoardChangeActivityStrategy)
            .Setup(x => x.GetChangeActivitiesAsync(cardId, listBoardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activities);

    private void Given_Assignees_activities(
        CardId cardId,
        PropertyValueChange<IReadOnlyCollection<AssigneeId>> assigneeIds,
        IReadOnlyCollection<Activity> activities)
        => Mock.Get(_assigneesChangeActivityStrategy)
            .Setup(x => x.GetChangeActivitiesAsync(cardId, assigneeIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activities);

    private void Given_Labels_activities(
        CardId cardId,
        PropertyValueChange<IReadOnlyCollection<LabelId>> labelIds,
        IReadOnlyCollection<Activity> activities)
        => Mock.Get(_labelsChangeActivityStrategy)
            .Setup(x => x.GetChangeActivitiesAsync(cardId, labelIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(activities);
}
