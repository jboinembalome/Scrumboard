using FluentAssertions;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Teams;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence;

public sealed class ScrumboardDbContextTests(
    DatabaseFixture databaseFixture) : PersistenceTestsBase(databaseFixture)
{
    [Fact]
    public async void SaveChangesAsync_should_set_auditable_properties()
    {
        // Arrange
        var board = new BoardDao
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
        
        DbContext.Boards.Add(board);
        
        // Act
        
        await DbContext.SaveChangesAsync();

        // Assert
        board.CreatedBy
            .Should()
            .Be("00000000-0000-0000-0000-000000000000");  // Value of the current user in DatabaseFixture
    }
}
