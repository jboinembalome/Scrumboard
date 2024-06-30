using FluentAssertions;
using Moq;
using Scrumboard.Application.Boards.Commands.DeleteBoard;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Application.UnitTests.Mocks;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards.Commands;

public class DeleteBoardCommandHandlerTests
{
    private readonly Mock<IBoardsRepository> _mockBoardRepository;

    public DeleteBoardCommandHandlerTests()
    {
        _mockBoardRepository = RepositoryMocks.GetBoardRepository();
    }

    [Fact]
    public async Task DeleteBoardTest_ExistingBoardId_BoardDeleted()
    {
        // Arrange
        var handler = new DeleteBoardCommandHandler(_mockBoardRepository.Object);
        var deleteBoardCommand = new DeleteBoardCommand { BoardId = 1 };

        // Act
        await handler.Handle(deleteBoardCommand, CancellationToken.None);
        //var allBoards = await _mockBoardRepository.Object.GetAsync(CancellationToken.None);

        // Assert
        //allBoards.Count.Should().Be(1);
    }

    [Fact]
    public async Task DeleteBoardTest_NoExistingBoardId_ThrowsAnExceptionNotFound()
    {
        // Arrange
        var handler = new DeleteBoardCommandHandler(_mockBoardRepository.Object);
        var deleteBoardCommand = new DeleteBoardCommand { BoardId = 0 };

        // Act
        Func<Task> action = async () => { await handler.Handle(deleteBoardCommand, CancellationToken.None); };

        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
