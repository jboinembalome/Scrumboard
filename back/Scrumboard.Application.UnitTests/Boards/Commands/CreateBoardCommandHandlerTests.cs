using AutoMapper;
using FluentAssertions;
using Moq;
using Scrumboard.Application.Boards;
using Scrumboard.Application.Boards.Commands.CreateBoard;
using Scrumboard.Application.Common.Profiles;
using Scrumboard.Application.UnitTests.Mocks;
using Scrumboard.Infrastructure.Abstractions.Common;
using Scrumboard.Infrastructure.Abstractions.Identity;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards.Commands;

public class CreateBoardCommandHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IBoardsRepository> _mockBoardRepository;
    private readonly IIdentityService _mockIdentityService;
    private readonly ICurrentUserService _currentUserService;
    
    public CreateBoardCommandHandlerTests()
    {
        _mockBoardRepository = RepositoryMocks.GetBoardRepository();
        _mockIdentityService = Mock.Of<IIdentityService>();
        _currentUserService = Mock.Of<ICurrentUserService>();
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
        var handler = new CreateBoardCommandHandler(_mapper, _mockBoardRepository.Object, _mockIdentityService, _currentUserService);
        var createBoardCommand = new CreateBoardCommand { CreatorId = "2cd08f87-33a6-4cbc-a0de-71d428986b85" };

        // Act
        var result = await handler.Handle(createBoardCommand, CancellationToken.None);
        var allBoards = await _mockBoardRepository.Object.TryGetByIdAsync(result.Board.Id);

        // Assert
        result.Success.Should().BeTrue();
    }
}
