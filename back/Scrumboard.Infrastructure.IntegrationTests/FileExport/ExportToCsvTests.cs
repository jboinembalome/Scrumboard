using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.FileExport;
using Scrumboard.Infrastructure.FileExport;
using Xunit;

namespace Scrumboard.Infrastructure.IntegrationTests.FileExport;

public class ExportToCsvTests
{
    private readonly ICsvExporter<Board> _csvExporter = new CsvExporter<Board>();

    [Fact]
    public async Task ExportToCsv_ListOfBoards_ReturnCsv()
    {
        // Arrange
        var board = new Board(
            name: "Scrumboard Frontend",
            isPinned: false,
            boardSetting:  new BoardSetting(
                colour: Colour.Violet),
            ownerId: new OwnerId("533f27ad-d3e8-4fe7-9259-ee4ef713dbea"));

        // Act
        var file = await _csvExporter.ExportToCsvAsync([board]);

        // Assert
        file.Should().NotBeEmpty();
    }
}
