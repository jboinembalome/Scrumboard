using AutoMapper;
using FluentAssertions;
using Moq;
using Scrumboard.Application.Boards.Commands.DeleteBoard;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Application.Common.Profiles;
using Scrumboard.Application.UnitTests.Mocks;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards.Commands;

public class DeleteBoardCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IAsyncRepository<Board, int>> _mockBoardRepository;

    public DeleteBoardCommandHandlerTests()
    {
        _mockBoardRepository = RepositoryMocks.GetBoardRepository();
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = configurationProvider.CreateMapper();
    }

    [Fact]
    public async Task DeleteBoardTest_ExistingBoardId_BoardDeleted()
    {
        // Arrange
        var handler = new DeleteBoardCommandHandler(_mapper, _mockBoardRepository.Object);
        var deleteBoardCommand = new DeleteBoardCommand { BoardId = 1 };

        // Act
        await handler.Handle(deleteBoardCommand, CancellationToken.None);
        var allBoards = await _mockBoardRepository.Object.ListAllAsync(CancellationToken.None);

        // Assert
        allBoards.Count.Should().Be(1);
    }

    [Fact]
    public async Task DeleteBoardTest_NoExistingBoardId_ThrowsAnExceptionNotFound()
    {
        // Arrange
        var handler = new DeleteBoardCommandHandler(_mapper, _mockBoardRepository.Object);
        var deleteBoardCommand = new DeleteBoardCommand { BoardId = 0 };

        // Act
        Func<Task> action = async () => { await handler.Handle(deleteBoardCommand, CancellationToken.None); };

        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
