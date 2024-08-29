using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Teams;
using Scrumboard.Infrastructure.Abstractions.Persistence.Teams;
using Scrumboard.Infrastructure.Persistence.Teams;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence.Teams;

public sealed class TeamsRepositoryTests : PersistenceTestsBase
{
    private readonly IFixture _fixture;
    private readonly ITeamsRepository _sut;
    
    public TeamsRepositoryTests(DatabaseFixture databaseFixture) 
        : base(databaseFixture)
    {
        _fixture = new CustomizedFixture();
        _sut = new TeamsRepository(ActDbContext);
    }

    [Fact]
    public async Task Should_add_Team()
    {           
        // Arrange
        var board = await Given_a_Board();
        
        var memberIds = _fixture.CreateMany<MemberId>().ToArray();
        
        var team = _fixture.Create<Team>();
        team.SetProperty(x => x.BoardId, board.Id);
        team.AddMembers(memberIds);

        // Act
        await _sut.AddAsync(team);
        await ActDbContext.SaveChangesAsync();
        
        // Assert
        var createdTeam = await AssertDbContext.Teams
            .Include(x => x.Members)
            .FirstAsync(x => x.Name == team.Name);

        createdTeam.Id.Value.Should().BeGreaterThan(0);
        createdTeam.Name.Should().Be(team.Name);
        createdTeam.BoardId.Should().Be(board.Id);
        createdTeam.Members.Should().BeEquivalentTo(team.Members);
    }
    
    [Fact]
    public async Task Should_delete_Team()
    {           
        // Arrange
        var team = await Given_a_Team();
        
        // Act
        await _sut.DeleteAsync(team.Id);
        await ActDbContext.SaveChangesAsync();
    
        // Assert
        var teamExists = await AssertDbContext.Teams
            .AnyAsync(x => x.Id == team.Id);
        
        teamExists.Should()
            .BeFalse();
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
            .NotBeNull();

        team!.Id.Should()
            .Be(existingTeam.Id);
    }
    
    
    [Fact]
    public async Task Should_update_Team()
    {           
        // Arrange
        var memberIds = _fixture.CreateMany<MemberId>().ToArray();
        
        var team = await Given_a_Team_for_edition();
        team.SetProperty(x => x.Name, _fixture.Create<string>());
        team.UpdateMembers(memberIds);
        
        // Act
        _sut.Update(team);
        await ActDbContext.SaveChangesAsync();
        
        // Assert
        var updatedTeam = await AssertDbContext.Teams
            .Include(x => x.Members)
            .FirstAsync(x => x.Id == team.Id);
        
        updatedTeam.Name.Should().Be(team.Name);
        updatedTeam.Members.Should().BeEquivalentTo(team.Members);
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
    
    private async Task<Team> Given_a_Team_for_edition()
    {
        var team = await Given_a_Team();
        
        return await ActDbContext.Teams
            .Include(x => x.Members)
            .FirstAsync(x => x.Id == team.Id);
    }

    private async Task<Board> Given_a_Board()
    {
        var board = _fixture.Create<Board>();
        
        ArrangeDbContext.Boards.Add(board);
        
        await ArrangeDbContext.SaveChangesAsync();
        
        return board;
    }
}
