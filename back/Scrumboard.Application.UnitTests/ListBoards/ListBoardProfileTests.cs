using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Application.ListBoards;
using Scrumboard.Domain.ListBoards;
using Xunit;

namespace Scrumboard.Application.UnitTests.ListBoards;

public sealed class ListBoardProfileTests : UnitTestsBase
{
    private readonly IFixture _fixture;
   
    private readonly IMapper _mapper;
    
    public ListBoardProfileTests()
    {
        _fixture = new Fixture();
        
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ListBoardProfile>();
        });
        
        _mapper = mapperConfiguration.CreateMapper();
    }
    
    [Fact]
    public void Should_map_ListBoardCreation_to_ListBoard()
    {
        // Arrange
        var listBoardCreation = _fixture.Create<ListBoardCreation>();
        
        // Act
        var listBoard = _mapper.Map<ListBoard>(listBoardCreation);
        
        // Assert
        var expectedListBoard = new ListBoard(
            name: listBoardCreation.Name,
            position: listBoardCreation.Position,
            boardId: listBoardCreation.BoardId);

        listBoard.Should()
            .BeEquivalentTo(expectedListBoard);
    }
}
