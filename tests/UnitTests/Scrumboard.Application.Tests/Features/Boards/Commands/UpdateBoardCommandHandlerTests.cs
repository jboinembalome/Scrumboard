using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Boards.Commands.UpdateBoard;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Application.Profiles;
using Scrumboard.Application.Tests.Mocks;
using Scrumboard.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Scrumboard.Application.Tests.Features.Boards.Commands
{
    public class UpdateBoardCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAsyncRepository<Board, Guid>> _mockBoardRepository;

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
            var updateBoardCommand = new UpdateBoardCommand { Name = "My new name", BoardId = Guid.Parse("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE") };

            // Act
            var result = await handler.Handle(updateBoardCommand, CancellationToken.None);
            var updatedBoard = await _mockBoardRepository.Object.GetByIdAsync(Guid.Parse("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE"), CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            updatedBoard.Name.Should().Be("My new name");
        }

        [Fact]
        public async Task UpdateBoardTest_NoExistingBoardId_ThrowsAnExceptionNotFound()
        {
            // Arrange
            var handler = new UpdateBoardCommandHandler(_mapper, _mockBoardRepository.Object);
            var updateBoardCommand = new UpdateBoardCommand { Name = "My new name", BoardId = Guid.Parse("C0788D2F-8003-43C1-92A4-EDC76A7C5DDE") };

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(updateBoardCommand, CancellationToken.None));
        }

        [Theory()]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task UpdateBoardTest_EmptyName_ThrowsAnExceptionValidation(string name)
        {
            // Arrange
            var handler = new UpdateBoardCommandHandler(_mapper, _mockBoardRepository.Object);
            var updateBoardCommand = new UpdateBoardCommand { Name = name, BoardId = Guid.Parse("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE") };

            // Act and Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(updateBoardCommand, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateBoardTest_NameGreaterthan50_ThrowsAnExceptionValidation()
        {
            // Arrange
            var handler = new UpdateBoardCommandHandler(_mapper, _mockBoardRepository.Object);
            string name = "aaaaaaaaaa";

            for (int i = 0; i < 20; i++)
                name += name;

            var updateBoardCommand = new UpdateBoardCommand { Name = name, BoardId = Guid.Parse("B0788D2F-8003-43C1-92A4-EDC76A7C5DDE") };

            // Act and Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(updateBoardCommand, CancellationToken.None));
        }
    }
}
