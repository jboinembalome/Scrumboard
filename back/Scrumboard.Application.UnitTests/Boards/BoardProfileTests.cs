using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Application.Abstractions.Boards;
using Scrumboard.Application.Boards;
using Scrumboard.Application.Teams;
using Scrumboard.Domain.Boards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards;

public sealed class BoardProfileTests : UnitTestsBase
{
    private readonly IFixture _fixture;
   
    private readonly IMapper _mapper;
    
    public BoardProfileTests()
    {
        _fixture = new Fixture();
        
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BoardProfile>();
            cfg.AddProfile<TeamProfile>();
        });
        
        _mapper = mapperConfiguration.CreateMapper();
    }
    
    [Fact]
    public void Should_map_BoardCreation_to_Board()
    {
        // Arrange
        var boardCreation = _fixture.Create<BoardCreation>();
        
        // Act
        var board = _mapper.Map<Board>(boardCreation);
        
        // Assert
        var expectedBoard = new Board(
            name: boardCreation.Name,
            isPinned: boardCreation.IsPinned,
            boardSetting:  new BoardSetting(
                colour: boardCreation.BoardSetting.Colour),
            ownerId: boardCreation.OwnerId);

        board
            .Should()
            .BeEquivalentTo(expectedBoard);
    }
    
    [Fact]
    public void Should_map_BoardEdition_to_Board()
    {
        // Arrange
        var boardEdition = _fixture.Create<BoardEdition>();
        
        // Act
        var board = _mapper.Map<Board>(boardEdition);
        
        // Assert
        var expectedBoard = new Board();
        expectedBoard.SetProperty(x => x.Id, boardEdition.Id);
        expectedBoard.SetProperty(x => x.Name, boardEdition.Name);
        expectedBoard.SetProperty(x => x.IsPinned, boardEdition.IsPinned);
        expectedBoard.SetProperty(x => x.BoardSetting, new BoardSetting(
            colour: boardEdition.BoardSetting.Colour));

        board
            .Should()
            .BeEquivalentTo(expectedBoard);
    }
    
    [Fact]
    public void Should_map_BoardSettingCreation_to_BoardSetting()
    {
        // Arrange
        var boardSettingCreation = _fixture.Create<BoardSettingCreation>();
        
        // Act
        var boardSetting = _mapper.Map<BoardSetting>(boardSettingCreation);
        
        // Assert
        var expectedBoardSetting = new BoardSetting(
            colour: boardSettingCreation.Colour);

        boardSetting
            .Should()
            .BeEquivalentTo(expectedBoardSetting);
    }
    
    [Fact]
    public void Should_map_BoardSettingEdition_to_BoardSetting()
    {
        // Arrange
        var boardSettingEdition = _fixture.Create<BoardSettingEdition>();
        
        // Act
        var boardSetting = _mapper.Map<BoardSetting>(boardSettingEdition);
        
        // Assert
        var expectedBoardSetting = new BoardSetting(
            colour: boardSettingEdition.Colour);

        boardSetting
            .Should()
            .BeEquivalentTo(expectedBoardSetting);
    }
}
