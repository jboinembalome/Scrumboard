using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Application.Teams;
using Scrumboard.Domain.Teams;
using Scrumboard.Shared.TestHelpers.Extensions;
using Xunit;

namespace Scrumboard.Application.UnitTests.Teams;

public sealed class TeamProfileTests
{
    private readonly IFixture _fixture;
    
    private readonly IMapper _mapper;
    
    public TeamProfileTests()
    {
        _fixture = new Fixture();
        
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TeamProfile>();
        });
        
        _mapper = mapperConfiguration.CreateMapper();
    }

    [Fact]
    public void Should_map_TeamCreation_to_Team()
    {
        // Arrange
        var teamCreation = _fixture.Create<TeamCreation>();
        
        // Act
        var team = _mapper.Map<Team>(teamCreation);
        
        // Assert
        var expectedTeam = new Team(
            name: teamCreation.Name,
            boardId: teamCreation.BoardId,
            memberIds: teamCreation.MemberIds);

        team.Should()
            .BeEquivalentTo(expectedTeam);
    }

    [Fact]
    public void Should_map_TeamEdition_to_Team()
    {
        // Arrange
        var teamEdition = _fixture.Create<TeamEdition>();
        var team = _fixture.Create<Team>();
        
        // Act
        _mapper.Map(teamEdition, team);
        
        // Assert
        var expectedTeam = new Team(
            name: teamEdition.Name,
            boardId: team.BoardId,
            memberIds: []);

        expectedTeam.SetProperty(x => x.Id, team.Id);
        expectedTeam.SetProperty(x => x.CreatedBy, team.CreatedBy);
        expectedTeam.SetProperty(x => x.CreatedDate, team.CreatedDate);
        expectedTeam.UpdateMembers(teamEdition.MemberIds);
        
        team.Should()
            .BeEquivalentTo(expectedTeam);
    }
}
