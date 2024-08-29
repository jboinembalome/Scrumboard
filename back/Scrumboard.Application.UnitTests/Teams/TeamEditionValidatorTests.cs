using AutoFixture;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Application.Teams;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Application.UnitTests.Teams;

public sealed class TeamEditionValidatorTests
{
    private readonly CustomizedFixture _fixture = new();
    
    private readonly ITeamsRepository _teamsRepository = Mock.Of<ITeamsRepository>();
    
    private readonly IValidator<TeamEdition> _sut;

    public TeamEditionValidatorTests()
    {
        _sut = new TeamEditionValidator(_teamsRepository);
    }
    
    [Fact]
    public async Task Should_have_error_when_Name_exceed_255_characters()
    {
        // Arrange
        var teamEdition = _fixture.Build<TeamEdition>()
            .With(x => x.Name, new string('A', 256))
            .Create();
        
        // Act
        var result = await _sut.TestValidateAsync(teamEdition);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [Fact]
    public async Task Should_have_error_when_Id_not_found()
    {
        // Arrange
        var teamEdition = _fixture.Build<TeamEdition>()
            .With(x => x.Name, new string('A', 255))
            .Create();
        
        Given_a_not_found_Team(teamEdition.Id);
        
        // Act
        var result = await _sut.TestValidateAsync(teamEdition);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
    
    [Fact]
    public async Task Should_pass_validation()
    {
        // Arrange
        var teamEdition = _fixture.Build<TeamEdition>()
            .With(x => x.Name, new string('A', 255))
            .Create();
        
        Given_a_found_Team(teamEdition.Id);
        
        // Act
        var result = await _sut.TestValidateAsync(teamEdition);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    private void Given_a_found_Team(TeamId teamId)
    {
        var team = _fixture.Build<Team>()
            .With(x => x.Id, teamId)
            .Create();

        Mock.Get(_teamsRepository)
            .Setup(x => x.TryGetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(team);
    }
    
    private void Given_a_not_found_Team(TeamId teamId) 
        => Mock.Get(_teamsRepository)
            .Setup(x => x.TryGetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Team);
}
