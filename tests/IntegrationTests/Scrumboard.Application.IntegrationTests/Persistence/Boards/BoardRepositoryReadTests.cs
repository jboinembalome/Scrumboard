using FluentAssertions;
using Scrumboard.Domain.Entities;
using System.Threading.Tasks;
using Xunit;

namespace Scrumboard.Application.IntegrationTests.Persistence.Boards
{
    [Collection("Database collection")]
    public class BoardRepositoryReadTests : IAsyncLifetime
    {
        private readonly DatabaseFixture _database;

        public BoardRepositoryReadTests(DatabaseFixture database)
        {
            _database = database;
        }

        public async Task DisposeAsync() =>  await DatabaseFixture.ResetState();

        public Task InitializeAsync() => Task.CompletedTask;

        [Fact]
        public async Task GetByIdAsync_ExistingId_BoardRetrieved()
        {           
            // Arrange
            var testBoardName = "testBoard";
            var board = new Board { Name = testBoardName };
            var boardRepository =  _database.GetRepository<Board, int>();

            _database.DbContext.Boards.Add(board);
            await _database.DbContext.SaveChangesAsync();

            int boardId = board.Id;

            // Act
            var boardRetrived = await boardRepository.GetByIdAsync(boardId);

            // Assert
            boardRetrived.Name.Should().Be(board.Name);
            boardRetrived.Id.Should().BeGreaterThan(0);
        }
    }
}
