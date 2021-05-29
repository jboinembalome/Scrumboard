using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Entities;
using System.Threading.Tasks;
using Xunit;

namespace Scrumboard.Application.IntegrationTests.Persistence.Boards
{
    [Collection("Database collection")]
    public class BoardRepositoryDeleteTests : IAsyncLifetime
    {
        private readonly DatabaseFixture _database;

        public BoardRepositoryDeleteTests(DatabaseFixture database)
        {
            _database = database;
        }

        public async Task DisposeAsync() =>  await DatabaseFixture.ResetState();

        public Task InitializeAsync() => Task.CompletedTask;

        [Fact]
        public async Task DeleteAsync_ExistingBoard_BoardDeleted()
        {           
            // Arrange
            var testBoardName = "testBoard";
            var board = new Board { Name = testBoardName };
            var boardRepository =  _database.GetRepository<Board, int>();

            _database.DbContext.Boards.Add(board);
            await _database.DbContext.SaveChangesAsync();

            // Act
            await boardRepository.DeleteAsync(board);
            var boardDeleted = await _database.DbContext.Boards.FirstOrDefaultAsync(b => b.Name == testBoardName);

            // Assert
            boardDeleted.Should().BeNull();
        }
    }
}
