using FluentAssertions;
using Scrumboard.Application.Interfaces.FileExport;
using Scrumboard.Infrastructure.FileExport;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scrumboard.Domain.Boards;
using Xunit;

namespace Scrumboard.Application.IntegrationTests.FileExport
{
    public class ExportToCsvTests
    {
        private readonly ICsvExporter<Board> _csvExporter;

        public ExportToCsvTests()
        {
            _csvExporter = new CsvExporter<Board>();
        }

        [Fact]
        public async Task ExportToCsv_ListOfBoards_ReturnCsv()
        {
            // Arrange
            var board1 = new Board { Name = "board1" };
            var board2 = new Board { Name = "board2" };
            var board3 = new Board { Name = "board3" };

            var boards = new List<Board> { board1, board2, board3};

            // Act
            var file = await _csvExporter.ExportToCsvAsync(boards);

            // Assert
            file.Should().NotBeEmpty();
        }
    }
}
