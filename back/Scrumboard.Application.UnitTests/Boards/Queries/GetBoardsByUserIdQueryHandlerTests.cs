using AutoMapper;
using Scrumboard.Application.Boards;
using Scrumboard.Application.Common.Profiles;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards.Queries;

public class GetBoardsByUserIdQueryHandlerTests
{
    private readonly IMapper _mapper;
   
    public GetBoardsByUserIdQueryHandlerTests()
    {
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
            cfg.AddProfile<BoardProfile>();
        });

        _mapper = configurationProvider.CreateMapper();
    }

    [Fact]
    public void GetBoardsByUserIdTest_ExistingUserId_ReturnsBoards()
    {
        // // Arrange
        // var handler = new GetBoardsByUserIdQueryHandler(_mapper, _mockBoardRepository.Object);
        // var getBoardsByUserIdQuery = new GetBoardsByUserIdQuery { UserId = Guid.Parse("2cd08f87-33a6-4cbc-a0de-71d428986b85") };
        //
        // // Act
        // var result = await handler.Handle(getBoardsByUserIdQuery, CancellationToken.None);
        //
        // // Assert
        // result.Should().BeAssignableTo<IEnumerable<BoardDto>>();
        // result.Should().HaveCount(2);
    }

    [Fact]
    public void GetBoardsByUserIdTest_NoExistingUserId_ReturnsZeroBoard()
    {
        // // Arrange
        // var handler = new GetBoardsByUserIdQueryHandler(_mapper, _mockBoardRepository.Object);
        // var getBoardsByUserIdQuery = new GetBoardsByUserIdQuery { UserId = Guid.Parse("3cd08f87-33a6-4cbc-a0de-71d428986b85") };
        //
        // // Act
        // var result = await handler.Handle(getBoardsByUserIdQuery, CancellationToken.None);
        //
        // // Assert
        // result.Should().BeAssignableTo<IEnumerable<BoardDto>>();
        // result.Should().HaveCount(0);
    }
}
