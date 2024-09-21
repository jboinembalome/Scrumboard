using AutoFixture;
using FluentAssertions;
using Moq;
using Scrumboard.Application.Cards.Activities.ActivityStrategies;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.Cards.Activities;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.ListBoards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards.Activities.ActivityStrategies;

public sealed class ListBoardActivityStrategyTests
{
    private readonly IFixture _fixture = new CustomizedFixture();
    
    private readonly IListBoardsRepository _listBoardsRepository;
    
    private readonly ListBoardActivityStrategy _sut;

    public ListBoardActivityStrategyTests()
    {
        _listBoardsRepository = Mock.Of<IListBoardsRepository>();

        _sut = new ListBoardActivityStrategy(_listBoardsRepository);
    }
    
    [Fact]
    public async Task Should_return_empty_when_no_changes()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var listBoardChange = (OldValue: new ListBoardId(1), NewValue: new ListBoardId(1));
        
        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, listBoardChange, CancellationToken.None);
        
        // Assert
        activities.Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Should_return_updated_Activity_when_ListBoard_has_been_updated()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var listBoardChange = (OldValue: new ListBoardId(1), NewValue: new ListBoardId(2));
        
        var newListBoard = Given_a_ListBoard(listBoardChange.NewValue.Value);

        // Act
        var activities = await _sut.GetChangeActivitiesAsync(cardId, listBoardChange, CancellationToken.None);

        // Assert
        var expectedActivities = new List<Activity>
        {
            new(
                cardId,
                ActivityType.Updated,
                ActivityField.ListBoard,
                string.Empty,
                newListBoard.Name)
        };

        activities.Should()
            .BeEquivalentTo(expectedActivities);
    }
    
    private ListBoard Given_a_ListBoard(ListBoardId listBoardId)
    {
        var listBoard = _fixture.Create<ListBoard>();
        listBoard.SetProperty(x => x.Id, listBoardId);
        listBoard.SetProperty(x => x.Name, _fixture.Create<string>());
        
        Mock.Get(_listBoardsRepository)
            .Setup(x => x.TryGetByIdAsync(listBoardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listBoard);

        return listBoard;
    }
}
