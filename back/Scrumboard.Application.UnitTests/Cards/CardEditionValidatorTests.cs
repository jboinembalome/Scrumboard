using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Cards;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.SharedKernel.Types;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards;

public sealed class CardEditionValidatorTests : CardInputBaseValidatorTestsBase<CardEdition>
{
    private readonly ICardsRepository _cardsRepository;

    private readonly IValidator<CardEdition> _sut;

    public CardEditionValidatorTests()
    {
        _cardsRepository = Mock.Of<ICardsRepository>();

        _sut = new CardEditionValidator(
            _cardsRepository,
            _listBoardsRepository,
            _labelsRepository,
            _identityService
        );
    }

    protected override IValidator<CardEdition> GetValidator() => _sut;


    [Fact]
    public async Task Should_have_error_when_CardId_not_found()
    {
        // Arrange
        var sut = GetValidator();

        var cardInput = _fixture.Build<CardEdition>()
            .With(x => x.Name, string.Empty)
            .Create();

        var userIds = cardInput.AssigneeIds
           .Select(x => (UserId)x.Value)
           .ToArray();

        Given_a_found_ListBoard(cardInput.ListBoardId);

        Given_found_Users(userIds);

        Given_found_Labels(cardInput.LabelIds);

        Given_a_not_found_Card(cardInput.Id);

        // Act
        var result = await _sut.TestValidateAsync(cardInput);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public async Task Should_pass_validation_for_valid_input()
    {
        // Arrange
        var sut = GetValidator();

        var cardInput = _fixture.Create<CardEdition>();

        var userIds = cardInput.AssigneeIds
           .Select(x => (UserId)x.Value)
           .ToArray();

        Given_a_found_ListBoard(cardInput.ListBoardId);

        Given_found_Users(userIds);

        Given_found_Labels(cardInput.LabelIds);

        Given_a_found_Card(cardInput.Id);
        // Act
        var result = await sut.TestValidateAsync(cardInput);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private void Given_a_found_Card(CardId cardId)
    {
        var card = _fixture.Create<Card>();
        card.SetProperty(x => x.Id, cardId);

        Mock.Get(_cardsRepository)
            .Setup(x => x.TryGetByIdAsync(cardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(card);
    }

    private void Given_a_not_found_Card(CardId cardId)
        => Mock.Get(_cardsRepository)
            .Setup(x => x.TryGetByIdAsync(cardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Card);
}
