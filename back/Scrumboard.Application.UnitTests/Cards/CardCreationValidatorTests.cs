using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Scrumboard.Application.Abstractions.Cards;
using Scrumboard.Application.Cards;
using Scrumboard.SharedKernel.Types;
using Xunit;

namespace Scrumboard.Application.UnitTests.Cards;

public sealed class CardCreationValidatorTests : CardValidatorTestsBase<CardCreation>
{
    private readonly IValidator<CardCreation> _sut;

    public CardCreationValidatorTests()
    {
        _sut = new CardCreationValidator(
            _listBoardsRepository,
            _labelsRepository,
            _identityService
        );
    }

    protected override IValidator<CardCreation> GetValidator() => _sut;

    [Fact]
    public async Task Should_pass_validation_for_valid_input()
    {
        // Arrange
        var sut = GetValidator();

        var cardInput = _fixture.Create<CardCreation>();

        var userIds = cardInput.AssigneeIds
           .Select(x => (UserId)x.Value)
           .ToArray();

        Given_a_found_ListBoard(cardInput.ListBoardId);

        Given_found_Users(userIds);

        Given_found_Labels(cardInput.LabelIds);

        // Act
        var result = await sut.TestValidateAsync(cardInput);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
