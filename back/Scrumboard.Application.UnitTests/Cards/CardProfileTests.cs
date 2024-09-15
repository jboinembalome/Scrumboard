using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Cards;
using Scrumboard.Domain.Cards;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards;

public sealed class CardProfileTests
{
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;

    public CardProfileTests()
    {
        _fixture = new Fixture();

        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CardProfile>();
        });

        _mapper = mapperConfiguration.CreateMapper();
    }

    [Fact]
    public void Should_map_CardCreation_to_Card()
    {
        // Arrange
        var cardCreation = _fixture.Create<CardCreation>();

        // Act
        var card = _mapper.Map<Card>(cardCreation);

        // Assert
        var expectedCard = new Card(
            name: cardCreation.Name,
            description: cardCreation.Description,
            dueDate: cardCreation.DueDate,
            position: cardCreation.Position,
            listBoardId: cardCreation.ListBoardId,
            assigneeIds: cardCreation.AssigneeIds,
            labelIds: cardCreation.LabelIds);

        card.Should().BeEquivalentTo(expectedCard);
    }
}
