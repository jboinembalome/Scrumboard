using AutoMapper;
using FluentAssertions;
using Moq;
using Scrumboard.Application.Features.Boards.Commands.CreateBoard;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Application.Profiles;
using Scrumboard.Application.UnitTests.Mocks;
using Scrumboard.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Scrumboard.Application.UnitTests.Features.Boards.Commands
{
    public class CreateBoardCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAsyncRepository<Board, int>> _mockBoardRepository;

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
            var createBoardCommand = new CreateBoardCommand { UserId = "2cd08f87-33a6-4cbc-a0de-71d428986b85" };

            // Act
            var result = await handler.Handle(createBoardCommand, CancellationToken.None);
            var allBoards = await _mockBoardRepository.Object.ListAllAsync(CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            allBoards.Count.Should().Be(3);
            allBoards[allBoards.Count - 1].Name.Should().Be(result.Board.Name);
        }
    }
}
