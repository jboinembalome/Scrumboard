using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Boards;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards;

public sealed class BoardCreationValidatorTests : UnitTestsBase
{
    private readonly CustomizedFixture _fixture = new();
    private readonly IValidator<BoardCreation> _sut = new BoardCreationValidator();

    [Fact]
    public async Task Should_have_error_when_Name_exceed_50_characters()
    {
        // Arrange
        var boardCreation = _fixture.Build<BoardCreation>()
            .With(x => x.Name, new string('A', 51))
            .Create();
        
        // Act
        var result = await _sut.TestValidateAsync(boardCreation);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [Fact]
    public async Task Should_pass_validation()
    {
        // Arrange
        var boardCreation = _fixture.Build<BoardCreation>()
            .With(x => x.Name, new string('A', 50))
            .Create();
        
        // Act
        var result = await _sut.TestValidateAsync(boardCreation);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
