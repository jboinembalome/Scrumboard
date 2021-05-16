using AutoMapper;
using FluentAssertions;
using Moq;
using Scrumboard.Application.Features.Boards.Commands.CreateBoard;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Application.Profiles;
using Scrumboard.Application.Tests.Mocks;
using Scrumboard.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Scrumboard.Application.Tests.Features.Boards.Commands
{
    public class CreateBoardCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAsyncRepository<Board, Guid>> _mockBoardRepository;

        public CreateBoardCommandHandlerTests()
        {
            _mockBoardRepository = RepositoryMocks.GetBoardRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task CreateBoardTest_ValidBoard_BoardAdded()
        {
            // Arrange
            var handler = new CreateBoardCommandHandler(_mapper, _mockBoardRepository.Object);
            var createBoardCommand = new CreateBoardCommand { Name = "My new board", UserId = Guid.Parse("2cd08f87-33a6-4cbc-a0de-71d428986b85") };

            // Act
            var result = await handler.Handle(createBoardCommand, CancellationToken.None);
            var allBoards = await _mockBoardRepository.Object.ListAllAsync(CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            allBoards.Count.Should().Be(3);
            allBoards.Last().Name.Should().Be(result.Board.Name);
        }

        [Theory()]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task CreateBoardTest_EmptyName_ReturnsValidationErrors(string name)
        {
            // Arrange
            var handler = new CreateBoardCommandHandler(_mapper, _mockBoardRepository.Object);
            var createBoardCommand = new CreateBoardCommand { Name = name, UserId = Guid.Parse("2cd08f87-33a6-4cbc-a0de-71d428986b85") };

            // Act
            var result = await handler.Handle(createBoardCommand, CancellationToken.None);
            var allBoards = await _mockBoardRepository.Object.ListAllAsync(CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.ValidationErrors.Count.Should().BeGreaterThan(0);
            allBoards.Count.Should().Be(2);
        }

        [Fact]
        public async Task CreateBoardTest_NameGreaterthan50_ReturnsValidationErrors()
        {
            // Arrange
            var handler = new CreateBoardCommandHandler(_mapper, _mockBoardRepository.Object);
            string name = "aaaaaaaaaa";

            for (int i = 0; i < 20; i++)
                name += name;

            var createBoardCommand = new CreateBoardCommand { Name = name, UserId = Guid.Parse("2cd08f87-33a6-4cbc-a0de-71d428986b85") };

            // Act
            var result = await handler.Handle(createBoardCommand, CancellationToken.None);
            var allBoards = await _mockBoardRepository.Object.ListAllAsync(CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.ValidationErrors.Count.Should().BeGreaterThan(0);
            allBoards.Count.Should().Be(2);
        }
    }
}
