using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using Scrumboard.Application.Boards.Commands.UpdateBoard;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Application.Common.Profiles;
using Scrumboard.Application.UnitTests.Mocks;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards.Commands;

public class UpdateBoardCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IAsyncRepository<Board, int>> _mockBoardRepository;

    public UpdateBoardCommandHandlerTests()
    {
        _mockBoardRepository = RepositoryMocks.GetBoardRepository();
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = configurationProvider.CreateMapper();
    }

    [Fact]
    public async Task UpdateBoardTest_ExistingBoardId_BoardUpdated()
    {
        // Arrange
        var handler = new UpdateBoardCommandHandler(_mapper, _mockBoardRepository.Object);
        var updateBoardCommand = new UpdateBoardCommand { Name = "My new name", BoardId = 1 };

        // Act
        var result = await handler.Handle(updateBoardCommand, CancellationToken.None);
        var updatedBoard = await _mockBoardRepository.Object.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        updatedBoard.Name.Should().Be("My new name");
    }

    [Fact]
    public async Task UpdateBoardTest_NoExistingBoardId_ThrowsAnExceptionNotFound()
    {
        // Arrange
        var handler = new UpdateBoardCommandHandler(_mapper, _mockBoardRepository.Object);
        var updateBoardCommand = new UpdateBoardCommand { Name = "My new name", BoardId = 0 };

        // Act
        Func<Task> action = async () => { await handler.Handle(updateBoardCommand, CancellationToken.None); };

        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}