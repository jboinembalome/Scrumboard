using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Boards.Commands.UpdateBoard;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Application.Profiles;
using Scrumboard.Application.UnitTests.Mocks;
using Scrumboard.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Scrumboard.Application.UnitTests.Features.Boards.Commands
{
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

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(updateBoardCommand, CancellationToken.None));
        }
    }
}
