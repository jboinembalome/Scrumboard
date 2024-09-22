using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Cards;
using Scrumboard.Domain.Cards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.SharedKernel.Exceptions;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards;

public sealed class CardsServiceTests
{
    private readonly IFixture _fixture;
    
    private readonly IMapper _mapper;
    private readonly ICardsRepository _cardsRepository;
    private readonly ICardsQueryRepository _cardsQueryRepository;
    private readonly IValidator<CardCreation> _cardCreationValidator;
    private readonly IValidator<CardEdition> _cardEditionValidator;

    private readonly CardsService _sut;

    public CardsServiceTests()
    {
        _fixture = new Fixture();
        
        _mapper = Mock.Of<IMapper>();
        _cardsRepository = Mock.Of<ICardsRepository>();
        _cardsQueryRepository = Mock.Of<ICardsQueryRepository>();
        _cardCreationValidator = Mock.Of<IValidator<CardCreation>>();
        _cardEditionValidator = Mock.Of<IValidator<CardEdition>>();

        _sut = new CardsService(
            _mapper,
            _cardsRepository,
            _cardsQueryRepository,
            _cardCreationValidator,
            _cardEditionValidator);
    }

    [Fact]
    public async Task Exists_Card_should_return_true_when_Card_exists()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        
        Given_a_found_Card(cardId);

        // Act
        var result = await _sut.ExistsAsync(cardId);

        // Assert
        result.Should()
            .BeTrue();
    }

    [Fact]
    public async Task Exists_Card_should_return_false_when_Card_does_not_exist()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        
        Given_a_not_found_Card(cardId);

        // Act
        var result = await _sut.ExistsAsync(cardId);

        // Assert
        result.Should()
            .BeFalse();
    }
    
    [Fact]
    public async Task Get_Cards_by_ListBoardId_should_return_Cards_when_found()
    {
        // Arrange
        var listBoardId = _fixture.Create<ListBoardId>();
        var cards = _fixture.CreateMany<Card>().ToArray();

        Mock.Get(_cardsQueryRepository)
            .Setup(x => x.GetByListBoardIdAsync(listBoardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cards);

        // Act
        var result = await _sut.GetByListBoardIdAsync(listBoardId);

        // Assert
        result.Should()
            .BeEquivalentTo(cards);
    }


    [Fact]
    public async Task Get_Card_by_Id_should_return_Card_when_found()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        var card = _fixture.Create<Card>();
        
        Mock.Get(_cardsQueryRepository)
            .Setup(x => x.TryGetByIdAsync(cardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(card);

        // Act
        var result = await _sut.GetByIdAsync(cardId);

        // Assert
        result.Should()
            .Be(card);
    }

    [Fact]
    public async Task Get_Card_by_Id_should_throw_an_exception_when_Card_not_found()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        
        Given_a_not_found_Card(cardId);

        // Act
        var act = async () => await _sut.GetByIdAsync(cardId);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Add_Card_should_validate_CardCreation()
    {
        // Arrange
        var cardCreation = _fixture.Create<CardCreation>();
        var card = _fixture.Create<Card>();
        
        Given_mapping(cardCreation, card);

        // Act
        await _sut.AddAsync(cardCreation);

        // Assert
        _cardCreationValidator.ShouldCallValidateAndThrowAsync(Times.Once());
    }

    [Fact]
    public async Task Add_Card_should_map_CardCreation_to_Card()
    {
        // Arrange
        var cardCreation = _fixture.Create<CardCreation>();
        var card = _fixture.Create<Card>();
        
        Given_mapping(cardCreation, card);

        // Act
        var result = await _sut.AddAsync(cardCreation);

        // Assert
        result.Should()
            .Be(card);
        
        Mock.Get(_mapper)
            .Verify(x => x.Map<Card>(cardCreation), Times.Once);
    }

    [Fact]
    public async Task Update_Card_should_validate_CardEdition()
    {
        // Arrange
        var cardEdition = _fixture.Create<CardEdition>();
        Given_a_found_Card(cardEdition.Id);

        // Act
        await _sut.UpdateAsync(cardEdition);

        // Assert
        _cardEditionValidator.ShouldCallValidateAndThrowAsync(Times.Once());
    }

    [Fact]
    public async Task Update_Card_should_update_properties()
    {
        // Arrange
        var cardEdition = _fixture.Create<CardEdition>();
        var card = Given_a_found_Card(cardEdition.Id);

        // Act
        await _sut.UpdateAsync(cardEdition);

        // Assert
        card.Name.Should().Be(cardEdition.Name);
        card.Description.Should().Be(cardEdition.Description);
        card.DueDate.Should().Be(cardEdition.DueDate);
        card.Position.Should().Be(cardEdition.Position);
        card.ListBoardId.Should().Be(cardEdition.ListBoardId);
        
        card.Assignees.Select(a => a.AssigneeId)
            .Should()
            .BeEquivalentTo(cardEdition.AssigneeIds);
        
        card.Labels.Select(l => l.LabelId)
            .Should()
            .BeEquivalentTo(cardEdition.LabelIds);
    }

    [Fact]
    public async Task Delete_Card_should_call_repository_when_found()
    {
        // Arrange
        var cardId = _fixture.Create<CardId>();
        
        Given_a_found_Card(cardId);

        // Act
        await _sut.DeleteAsync(cardId);

        // Assert
        Mock.Get(_cardsRepository)
            .Verify(x => x.DeleteAsync(cardId, It.IsAny<CancellationToken>()), Times.Once);
    }

    private void Given_mapping(CardCreation cardCreation, Card card) 
        => Mock.Get(_mapper)
            .Setup(x => x.Map<Card>(cardCreation))
            .Returns(card);
    
    private Card Given_a_found_Card(CardId cardId)
    {
        var card = _fixture.Create<Card>();
        card.SetProperty(x => x.Id, cardId);
        
        Mock.Get(_cardsRepository)
            .Setup(x => x.TryGetByIdAsync(cardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(card);
        
        return card;
    }

    private void Given_a_not_found_Card(CardId cardId)
        => Mock.Get(_cardsRepository)
            .Setup(x => x.TryGetByIdAsync(cardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Card);
}
