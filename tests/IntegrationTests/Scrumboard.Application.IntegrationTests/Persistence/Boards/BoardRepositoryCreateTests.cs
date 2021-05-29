using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Scrumboard.Domain.Entities;
using System.Threading.Tasks;
using Xunit;

namespace Scrumboard.Application.IntegrationTests.Persistence.Boards
{
    [Collection("Database collection")]
    public class BoardRepositoryCreateTests : IAsyncLifetime
    {
        private readonly DatabaseFixture _database;

        public BoardRepositoryCreateTests(DatabaseFixture database)
        {
            _database = database;
        }

        public async Task DisposeAsync() =>  await DatabaseFixture.ResetState();

        public Task InitializeAsync() => Task.CompletedTask;

        [Fact]
        public async Task AddAsync_ValidBoard_BoardAdded()
        {           
            // Arrange
            var testBoardName = "testBoard";
            var board = new Board { Name = testBoardName };
            var boardRepository =  _database.GetRepository<Board, int>();

            // Act
            var newboard = await boardRepository.AddAsync(board);
            var boardAdded = (await _database.DbContext.Boards.ToListAsync())[0];

            // Assert
            boardAdded.Name.Should().Be(newboard.Name);
            boardAdded.Id.Should().BeGreaterThan(0);
        }
    }
}
