
using FluentAssertions;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Teams;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.Persistence;

[Collection("Database collection")]
public class ScrumboardDbContextTests : IAsyncLifetime
{
    private readonly DatabaseFixture _database;

    public ScrumboardDbContextTests(DatabaseFixture database)
    {
        _database = database;
    }

    public async Task DisposeAsync() => await _database.ResetState();

    public Task InitializeAsync() => Task.CompletedTask;

    [Fact]
    public async void SaveChangesAsync_SetCreatedByProperty()
    {
        // Arrange
        var _currentUserService = "00000000-0000-0000-0000-000000000000"; // Value of the current user in DatabaseFixture
        var testBoardName = "testBoard";
        var board = new BoardDao
        {
            Name = testBoardName,
            BoardSetting = new BoardSettingDao
            {
                Colour = Colour.Gray
            },
            Team = new TeamDao
            {
                Name = "Team 1"
            }
        };

        _database.SetDbContext();

        // Act
        _database.DbContext!.Boards.Add(board);
        await _database.DbContext.SaveChangesAsync();

        // Assert
        board.CreatedBy.Should().Be(_currentUserService);
    }
}
