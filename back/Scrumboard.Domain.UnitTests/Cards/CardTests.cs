using AutoFixture;
using FluentAssertions;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Cards.Events;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;
using Scrumboard.Shared.TestHelpers.Extensions;

namespace Scrumboard.Domain.UnitTests.Cards;
public sealed class CardTests
{
    private readonly IFixture _fixture = new CustomizedFixture();

    [Fact]
    public void Update_should_throw_an_exception_when_Id_is_null()
    {
        // Arrange
        var card = Given_a_Card_on_creation();

        // Act
        var act = () => card.Update(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            dueDate: _fixture.Create<DateTimeOffset>(),
            position: _fixture.Create<int>(),
            listBoardId: _fixture.Create<ListBoardId>(),
            assigneeIds: _fixture.CreateMany<AssigneeId>().ToArray(),
            labelIds: _fixture.CreateMany<LabelId>().ToArray());

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Cannot update a {nameof(Card)} when {nameof(Card.Id)} is null.");
    }

    [Fact]
    public void Update_should_correctly_apply_changes()
    {
        // Arrange
        var card = Given_a_Card_on_edition();

        var newName = _fixture.Create<string>();
        var newDescription = _fixture.Create<string>();
        var newDueDate = _fixture.Create<DateTimeOffset>();
        var newPosition = _fixture.Create<int>();
        var newListBoardId = _fixture.Create<ListBoardId>();
        var newAssigneeIds = _fixture.CreateMany<AssigneeId>().ToArray();
        var newLabelIds = _fixture.CreateMany<LabelId>().ToArray();

        // Act
        card.Update(
            name: newName,
            description: newDescription,
            dueDate: newDueDate,
            position: newPosition,
            listBoardId: newListBoardId,
            assigneeIds: newAssigneeIds,
            labelIds: newLabelIds
        );

        // Assert
        card.Name.Should().Be(newName);
        card.Description.Should().Be(newDescription);
        card.DueDate.Should().Be(newDueDate);
        card.Position.Should().Be(newPosition);
        card.ListBoardId.Should().Be(newListBoardId);

        card.Assignees.Should()
            .BeEquivalentTo(newAssigneeIds
                .Select(x => new CardAssignee { CardId = card.Id, AssigneeId = x }));

        card.Labels.Should()
            .BeEquivalentTo(newLabelIds
                .Select(x => new CardLabel { CardId = card.Id, LabelId = x }));
    }

    [Fact]
    public void Update_should_add_CardUpdatedDomainEvent()
    {
        // Arrange
        var card = Given_a_Card_on_edition();
        var newName = _fixture.Create<string>();
        var newDescription = _fixture.Create<string>();
        var newDueDate = _fixture.Create<DateTimeOffset>();
        var newPosition = _fixture.Create<int>();
        var newListBoardId = _fixture.Create<ListBoardId>();
        var newAssigneeIds = _fixture.CreateMany<AssigneeId>().ToList();
        var newLabelIds = _fixture.CreateMany<LabelId>().ToList();

        // Act
        card.Update(
            name: newName,
            description: newDescription,
            dueDate: newDueDate,
            position: newPosition,
            listBoardId: newListBoardId,
            assigneeIds: newAssigneeIds,
            labelIds: newLabelIds
        );

        // Assert
        var expectedCardUpdatedDomainEvent = new CardUpdatedDomainEvent(card.Id);

        card.DomainEvents.Should()
            .ContainSingle()
            .Which
                .Should()
                .BeEquivalentTo(expectedCardUpdatedDomainEvent, opt => opt
                    .Excluding(x => x.DateOccurred));
    }

    private Card Given_a_Card_on_creation() 
        => new(
            name: _fixture.Create<string>(),
            description: _fixture.Create<string>(),
            dueDate: _fixture.Create<DateTimeOffset>(),
            position: _fixture.Create<int>(),
            listBoardId: _fixture.Create<ListBoardId>(),
            assigneeIds: _fixture.CreateMany<AssigneeId>().ToList(),
            labelIds: _fixture.CreateMany<LabelId>().ToList());

    private Card Given_a_Card_on_edition()
    {
        var card = Given_a_Card_on_creation();

        card.SetProperty(x => x.Id, _fixture.Create<CardId>());

        return card;
    }

}
