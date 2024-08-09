using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Teams;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence;

public sealed class ScrumboardDbContextTests(
    DatabaseFixture databaseFixture) : PersistenceTestsBase(databaseFixture)
{
    [Fact]
    public async void SaveChangesAsync_should_set_auditable_properties_when_creation()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        SetCurrentUser(userId);

        var currentDate = DateTimeOffset.Now;
        SetCurrentDate(currentDate);
        
        var boardDao = BuildBoardDao();
        
        // Act
        ActDbContext.Boards.Add(boardDao);
        await ActDbContext.SaveChangesAsync();

        // Assert
        boardDao.CreatedBy
            .Should()
            .Be(userId);
        
        boardDao.CreatedDate
            .Should()
            .Be(currentDate);
    }

    [Fact]
    public async void SaveChangesAsync_should_set_auditable_properties_when_edition()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        SetCurrentUser(userId);

        var currentDate = DateTimeOffset.Now;
        SetCurrentDate(currentDate);
        
        var newBoardDao = await Given_a_BoardDao();
            
        // Act
        var boardDao = await ActDbContext.Boards.FirstAsync(x => x.Id == newBoardDao.Id);
        boardDao.Name = "Updated name";
        
        await ActDbContext.SaveChangesAsync();

        // Assert
        boardDao.LastModifiedBy
            .Should()
            .Be(userId);
        
        boardDao.LastModifiedDate
            .Should()
            .Be(currentDate);
    }

    private async Task<BoardDao> Given_a_BoardDao()
    {
        var boardDao = BuildBoardDao();
        
        ArrangeDbContext.Boards.Add(boardDao);
        await ArrangeDbContext.SaveChangesAsync();
        
        return boardDao;
    }

    private static BoardDao BuildBoardDao() 
        => new()
        {
            Name = "testBoard",
            BoardSetting = new BoardSettingDao
            {
                Colour = Colour.Gray
            },
            Team = new TeamDao
            {
                Name = "Team 1"
            }
        };
}
