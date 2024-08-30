using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Application.Abstractions.Teams;
using Scrumboard.Domain.Teams;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.Web.Api.Teams;
using Scrumboard.Web.Api.Users;
using Xunit;

namespace Scrumboard.Web.UnitTests.Api.Teams;

public sealed class TeamProfileTests
{
    private readonly IFixture _fixture;
   
    private readonly IMapper _mapper;

    public TeamProfileTests()
    {
        _fixture = new CustomizedFixture();
        
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TeamProfile>();
            cfg.SkipUserDtoMappings();
        });
        
        _mapper = mapperConfiguration.CreateMapper();
    }
    
    [Fact]
    public void Should_map_TeamCreationDto_to_TeamCreation()
    {
        // Arrange
        var teamCreationDto = _fixture.Create<TeamCreationDto>();
        
        // Act
        var teamCreation = _mapper.Map<TeamCreation>(teamCreationDto);
        
        // Assert
        var expectedTeamCreation = new TeamCreation
        {
            Name = teamCreationDto.Name,
            MemberIds = teamCreationDto.MemberIds
                .Select(x => (MemberId)x)
                .ToArray(),
            BoardId = teamCreationDto.BoardId
        };

        teamCreation.Should()
            .BeEquivalentTo(expectedTeamCreation);
    }
    
    [Fact]
    public void Should_map_TeamEditionDto_to_TeamEdition()
    {
        // Arrange
        var teamEditionDto = _fixture.Create<TeamEditionDto>();
        
        // Act
        var teamEdition = _mapper.Map<TeamEdition>(teamEditionDto);
        
        // Assert
        var expectedTeamEdition = new TeamEdition
        {
            Id = teamEditionDto.Id,
            Name = teamEditionDto.Name,
            MemberIds = teamEditionDto.MemberIds
                .Select(x => (MemberId)x)
                .ToArray(),
            BoardId = teamEditionDto.BoardId
        };

        teamEdition.Should()
            .BeEquivalentTo(expectedTeamEdition);
    }
    
    [Fact]
    public void Should_map_Team_to_TeamDto()
    {
        // Arrange
        var team = _fixture.Create<Team>();
        team.SetProperty(x => x.Name, _fixture.Create<string>());
        
        // Act
        var teamDto = _mapper.Map<TeamDto>(team);
        
        // Assert
        var expectedTeamDto = new TeamDto
        {
            Id = team.Id.Value,
            Name = team.Name
        };

        teamDto.Should()
            .BeEquivalentTo(expectedTeamDto);
    }
    
    [Fact]
    public void Should_map_TeamMember_to_UserDto()
    {
        // Arrange
        var teamMember = _fixture.Create<TeamMember>();
        
        // Act
        var userDto = _mapper.Map<UserDto>(teamMember);
        
        // Assert
        var expectedUserDto = new UserDto
        {
            Id = teamMember.MemberId.Value
        };

        userDto.Should()
            .BeEquivalentTo(expectedUserDto);
    }
}
