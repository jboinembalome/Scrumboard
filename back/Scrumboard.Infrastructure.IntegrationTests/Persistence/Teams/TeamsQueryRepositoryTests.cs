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
        var existingTeam = await Given_a_Team();
        
        // Act
        var team = await _sut.TryGetByIdAsync(existingTeam.Id);
    
        // Assert
        team.Should()
            .BeEquivalentTo(existingTeam);
    }
    
    [Fact]
    public async Task Should_get_Team_by_BoardId()
    {           
        // Arrange
        var existingTeam = await Given_a_Team();
        
        // Act
        var team = await _sut.TryGetByBoardIdAsync(existingTeam.BoardId);
    
        // Assert
        team.Should()
            .BeEquivalentTo(existingTeam);
    }
    
    private async Task<Team> Given_a_Team()
    {
        var board = await Given_a_Board();
        
        var team = _fixture.Create<Team>();
        team.SetProperty(x => x.BoardId, board.Id);
        
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
