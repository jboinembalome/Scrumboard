using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Application.Teams;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.SharedKernel.Exceptions;
using Xunit;

namespace Scrumboard.Application.UnitTests.Teams;

public sealed class TeamsServiceTests
{
    private readonly IFixture _fixture;
    
    private readonly IMapper _mapper;
    private readonly ITeamsRepository _teamsRepository;
    private readonly ITeamsQueryRepository _teamsQueryRepository;
    private readonly IValidator<TeamCreation> _teamCreationValidator;
    private readonly IValidator<TeamEdition> _teamEditionValidator;
    
    private readonly TeamsService _sut;
    
    public TeamsServiceTests()
    {
        _fixture = new Fixture();
        
        _mapper = Mock.Of<IMapper>();
        _teamsRepository = Mock.Of<ITeamsRepository>();
        _teamsQueryRepository = Mock.Of<ITeamsQueryRepository>();
        _teamCreationValidator = Mock.Of<IValidator<TeamCreation>>();
        _teamEditionValidator = Mock.Of<IValidator<TeamEdition>>();

        _sut = new TeamsService(
            _mapper,
            _teamsRepository,
            _teamsQueryRepository,
            _teamCreationValidator,
            _teamEditionValidator);
    }
    
    [Fact]
    public async Task Get_Team_by_Id_should_throw_exception_when_Team_not_found()
    {
        // Arrange
        var teamId = _fixture.Create<TeamId>();
        
        Mock.Get(_teamsQueryRepository)
            .Setup(x => x.TryGetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Team);

        // Act
        var act = async () => await _sut.GetByIdAsync(teamId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Get_Team_by_Id_should_return_Team_when_found()
    {
        // Arrange
        var teamId = _fixture.Create<TeamId>();
        var team = _fixture.Create<Team>();
        
        Mock.Get(_teamsQueryRepository)
            .Setup(x => x.TryGetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(team);

        // Act
        var result = await _sut.GetByIdAsync(teamId);

        // Assert
        result.Should().Be(team);
    }

    [Fact]
    public async Task Get_Team_by_BoardId_should_throw_exception_when_Team_not_found()
    {
        // Arrange
        var boardId = _fixture.Create<BoardId>();
        
        Mock.Get(_teamsQueryRepository)
            .Setup(x => x.TryGetByBoardIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Team);

        // Act
        var act = async () => await _sut.GetByBoardIdAsync(boardId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Get_Team_by_BoardId_should_return_Team_when_found()
    {
        // Arrange
        var boardId = _fixture.Create<BoardId>();
        var team = _fixture.Create<Team>();
        
        Mock.Get(_teamsQueryRepository)
            .Setup(x => x.TryGetByBoardIdAsync(boardId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(team);

        // Act
        var result = await _sut.GetByBoardIdAsync(boardId);

        // Assert
        result.Should().Be(team);
    }

    [Fact]
    public async Task Add_Team_should_validate_TeamCreation()
    {
        // Arrange
        var teamCreation = _fixture.Create<TeamCreation>();
        var team = _fixture.Create<Team>();
        
        Given_mapping(teamCreation, team);

        // Act
        await _sut.AddAsync(teamCreation);

        // Assert
        _teamCreationValidator.ShouldCallValidateAndThrowAsync(Times.Once());
    }

    [Fact]
    public async Task Add_Team_should_not_proceed_when_validation_fails()
    {
        // Arrange
        var teamCreation = _fixture.Build<TeamCreation>().Create();
        teamCreation.Name = string.Empty;
        
        _teamCreationValidator.SetupValidationFailed(
            propertyName: nameof(TeamCreation.Name), 
            errorMessage: "Name is required."
        );
        
        // Act
        var act = async () => await _sut.AddAsync(teamCreation);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        Mock.Get(_teamsRepository)
            .Verify(x => x.AddAsync(
                It.IsAny<Team>(), 
                It.IsAny<CancellationToken>()), 
                Times.Never);
    }

    [Fact]
    public async Task Add_Team_should_map_TeamCreation_to_Team()
    {
        // Arrange
        var teamCreation = _fixture.Create<TeamCreation>();
        var team = _fixture.Create<Team>();
        
        Given_mapping(teamCreation, team);

        // Act
        var result = await _sut.AddAsync(teamCreation);

        // Assert
        result.Should().Be(team);
        
        Mock.Get(_mapper)
            .Verify(x => x.Map<Team>(teamCreation), Times.Once);
    }

    [Fact]
    public async Task Add_Team_should_call_the_repository()
    {
        // Arrange
        var teamCreation = _fixture.Create<TeamCreation>();
        var team = _fixture.Create<Team>();
        
        Given_mapping(teamCreation, team);

        // Act
        await _sut.AddAsync(teamCreation);

        // Assert
        Mock.Get(_teamsRepository)
            .Verify(x => x.AddAsync(
                team, 
                It.IsAny<CancellationToken>()), 
                Times.Once);
    }

    [Fact]
    public async Task Update_Team_should_validate_TeamEdition()
    {
        // Arrange
        var teamEdition = _fixture.Create<TeamEdition>();
        var team = Given_a_found_Team(teamEdition.Id);

        Given_mapping(teamEdition, team);

        // Act
        await _sut.UpdateAsync(teamEdition);

        // Assert
        _teamEditionValidator.ShouldCallValidateAndThrowAsync(Times.Once());
    }

    [Fact]
    public async Task Update_Team_should_not_proceed_when_validation_fails()
    {
        // Arrange
        var teamEdition = _fixture.Build<TeamEdition>().Create();
        teamEdition.Name = string.Empty;
        
        _teamEditionValidator.SetupValidationFailed(
            propertyName: nameof(TeamEdition.Name), 
            errorMessage: "Name is required."
        );
        
        // Act
        var act = async () => await _sut.UpdateAsync(teamEdition);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        
        Mock.Get(_teamsRepository)
            .Verify(x => x.Update(
                It.IsAny<Team>()), 
                Times.Never);
    }

    [Fact]
    public async Task Update_Team_should_not_proceed_when_Team_not_found()
    {
        // Arrange
        var teamEdition = _fixture.Build<TeamEdition>().Create();
        
        Given_a_not_found_Team(teamEdition.Id);

        // Act
        var act = async () => await _sut.UpdateAsync(teamEdition);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        
        Mock.Get(_teamsRepository)
            .Verify(x => x.Update(
                It.IsAny<Team>()), 
                Times.Never);
    }

    [Fact]
    public async Task Update_Team_should_call_the_repository()
    {
        // Arrange
        var teamEdition = _fixture.Create<TeamEdition>();
        var team = Given_a_found_Team(teamEdition.Id);

        Given_mapping(teamEdition, team);

        // Act
        await _sut.UpdateAsync(teamEdition);

        // Assert
        Mock.Get(_teamsRepository)
            .Verify(x => x.Update(team), Times.Once);
    }

    [Fact]
    public async Task Delete_Team_should_not_proceed_when_Team_not_found()
    {
        // Arrange
        var teamId = _fixture.Create<TeamId>();
        
        Given_a_not_found_Team(teamId);

        // Act
        var act = async () => await _sut.DeleteAsync(teamId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        Mock.Get(_teamsRepository)
            .Verify(x => x.DeleteAsync(
                It.IsAny<TeamId>(), 
                It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Delete_Team_should_call_the_repository()
    {
        // Arrange
        var teamId = _fixture.Create<TeamId>();
        
        Given_a_found_Team(teamId);

        // Act
        await _sut.DeleteAsync(teamId);

        // Assert
        Mock.Get(_teamsRepository)
            .Verify(x => x.DeleteAsync(
                    teamId, 
                    It.IsAny<CancellationToken>()), 
                Times.Once);
    }
    
    private void Given_mapping(TeamCreation teamCreation, Team team) 
        => Mock.Get(_mapper)
            .Setup(x => x.Map<Team>(teamCreation))
            .Returns(team);

    private void Given_mapping(TeamEdition teamEdition, Team team) 
        => Mock.Get(_mapper)
            .Setup(x => x.Map(teamEdition, team))
            .Returns(team);

    private Team Given_a_found_Team(TeamId teamId)
    {
        var team = _fixture.Build<Team>()
            .With(x => x.Id, teamId)
            .Create();

        Mock.Get(_teamsRepository)
            .Setup(x => x.TryGetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(team);
        
        return team;
    }
    
    private void Given_a_not_found_Team(TeamId teamId) 
        => Mock.Get(_teamsRepository)
            .Setup(x => x.TryGetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Team);
}
