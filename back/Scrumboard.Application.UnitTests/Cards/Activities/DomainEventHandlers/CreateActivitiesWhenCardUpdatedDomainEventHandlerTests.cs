using AutoFixture;
using Moq;
using Scrumboard.Application.Cards.Activities;
using Scrumboard.Application.Cards.Activities.DomainEventHandlers;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.Cards.Events;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Activities;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards.Activities.DomainEventHandlers;

public sealed class CreateActivitiesWhenCardUpdatedDomainEventHandlerTests
{
    private readonly IFixture _fixture;

    private readonly IActivityFactory _activityFactory;
    private readonly IActivitiesRepository _activitiesRepository;

    private readonly CreateActivitiesWhenCardUpdatedDomainEventHandler _sut;

    public CreateActivitiesWhenCardUpdatedDomainEventHandlerTests()
    {
        _fixture = new CustomizedFixture();

        _activityFactory = Mock.Of<IActivityFactory>();
        _activitiesRepository = Mock.Of<IActivitiesRepository>();

        _sut = new CreateActivitiesWhenCardUpdatedDomainEventHandler(
            _activityFactory,
            _activitiesRepository);
    }

    [Fact]
    public async Task Should_add_Activities_when_CardUpdatedDomainEvent_is_handled()
    {
        // Arrange
        var domainEvent = _fixture.Create<CardUpdatedDomainEvent>();

        var newActivities = Given_some_activities(domainEvent);

        // Act
        await _sut.Handle(domainEvent, CancellationToken.None);

        // Assert
        Mock.Get(_activitiesRepository)
            .Verify(x => x.AddAsync(
                    newActivities,
                    It.IsAny<CancellationToken>()),
                Times.Once);
    }

    [Fact]
    public async Task Add_Activities_should_not_proceed_when_ActivityFactory_return_zero_activities()
    {
        // Arrange
        var domainEvent = _fixture.Create<CardUpdatedDomainEvent>();

        Given_no_activities(domainEvent);

        // Act
        await _sut.Handle(domainEvent, CancellationToken.None);

        // Assert
        Mock.Get(_activitiesRepository)
            .Verify(x => x.AddAsync(
                    It.IsAny<IEnumerable<Activity>>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
    }

    private Activity[] Given_some_activities(CardUpdatedDomainEvent domainEvent)
    {
        var newActivities = _fixture.CreateMany<Activity>().ToArray();

        Mock.Get(_activityFactory)
            .Setup(factory => factory.CreateAsync(domainEvent, It.IsAny<CancellationToken>()))
            .ReturnsAsync(newActivities);

        return newActivities;
    }

    private void Given_no_activities(CardUpdatedDomainEvent domainEvent)
        => Mock.Get(_activityFactory)
            .Setup(factory => factory.CreateAsync(domainEvent, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
}
