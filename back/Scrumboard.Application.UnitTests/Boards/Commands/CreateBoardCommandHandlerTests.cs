using AutoMapper;
using FluentAssertions;
using Moq;
using Scrumboard.Application.Boards;
using Scrumboard.Application.Boards.Commands.CreateBoard;
using Scrumboard.Application.Common.Profiles;
using Scrumboard.Application.UnitTests.Mocks;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards.Commands;

public class CreateBoardCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IAsyncRepository<Board, int>> _mockBoardRepository;
    private readonly IIdentityService _mockIdentityService;

    public CreateBoardCommandHandlerTests()
    {
        _mockBoardRepository = RepositoryMocks.GetBoardRepository();
        _mockIdentityService = Mock.Of<IIdentityService>();
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
            cfg.AddProfile<BoardProfile>();
        });

        _mapper = configurationProvider.CreateMapper();
    }

    [Fact]
    public async Task CreateBoardTest_ValidBoard_BoardAdded()
    {
        // Arrange
        var handler = new CreateBoardCommandHandler(_mapper, _mockBoardRepository.Object, _mockIdentityService);
        var createBoardCommand = new CreateBoardCommand { CreatorId = Guid.Parse("2cd08f87-33a6-4cbc-a0de-71d428986b85") };

        // Act
        var result = await handler.Handle(createBoardCommand, CancellationToken.None);
        var allBoards = await _mockBoardRepository.Object.ListAllAsync(CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        allBoards.Count.Should().Be(3);
        allBoards[allBoards.Count - 1].Name.Should().Be(result.Board.Name);
    }
}
