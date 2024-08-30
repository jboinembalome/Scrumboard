using AutoFixture;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;
using Scrumboard.Infrastructure.Persistence.Teams;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Teams;

public sealed class TeamsQueryRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    private readonly ITeamsQueryRepository _sut;
    
    public TeamsQueryRepositoryTests(DatabaseFixture databaseFixture) 
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        _sut = new TeamsQueryRepository(ActDbContext);
    }

    [Fact]
    public async Task Should_get_Team_by_Id()
    {           
        // Arrange
        var team = await Given_a_Team();
        
        // Act
        var retrievedTeam = await _sut.TryGetByIdAsync(team.Id);
    
        // Assert
        retrievedTeam.Should()
            .BeEquivalentTo(team);
    }
    
    [Fact]
    public async Task Get_Team_by_Id_should_return_null_when_not_found()
    {           
        // Act
        var retrievedTeam = await _sut.TryGetByIdAsync(new TeamId(0));

        // Assert
        retrievedTeam.Should()
            .BeNull();
    }
    
    [Fact]
    public async Task Should_get_Team_by_BoardId()
    {           
        // Arrange
        var team = await Given_a_Team();
        
        // Act
        var retrievedTeam = await _sut.TryGetByBoardIdAsync(team.BoardId);
    
        // Assert
        retrievedTeam.Should()
            .BeEquivalentTo(team);
    }
    
    [Fact]
    public async Task Get_Team_by_BoardId_should_return_null_when_not_found()
    {           
        // Act
        var retrievedTeam = await _sut.TryGetByBoardIdAsync(new BoardId(0));

        // Assert
        retrievedTeam.Should()
            .BeNull();
    }
    
    private async Task<Team> Given_a_Team()
    {
        var board = await Given_a_Board();
        var memberIds = _fixture.CreateMany<MemberId>().ToArray();
        
        var team = _fixture.Create<Team>();
        team.SetProperty(x => x.BoardId, board.Id);
        team.AddMembers(memberIds);
        
        ArrangeDbContext.Teams.Add(team);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return team;
    }
    
    private async Task<Board> Given_a_Board()
    {
        var board = _fixture.Create<Board>();
        
        ArrangeDbContext.Boards.Add(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
    }
}
