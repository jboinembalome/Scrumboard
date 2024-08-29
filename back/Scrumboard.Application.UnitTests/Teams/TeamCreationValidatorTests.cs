using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Application.Teams;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.Teams;

public sealed class TeamCreationValidatorTests
{
    private readonly CustomizedFixture _fixture = new();
    private readonly IValidator<TeamCreation> _sut = new TeamCreationValidator();
    
    [Fact]
    public async Task Should_have_error_when_Name_exceed_255_characters()
    {
        // Arrange
        var teamCreation = _fixture.Build<TeamCreation>()
            .With(x => x.Name, new string('A', 256))
            .Create();
        
        // Act
        var result = await _sut.TestValidateAsync(teamCreation);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [Fact]
    public async Task Should_pass_validation()
    {
        // Arrange
        var teamCreation = _fixture.Build<TeamCreation>()
            .With(x => x.Name, new string('A', 255))
            .Create();
        
        // Act
        var result = await _sut.TestValidateAsync(teamCreation);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
