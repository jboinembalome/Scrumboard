using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Teams;
using Xunit;

namespace Scrumboard.Infrastructure.UnitTests.Persistence.Boards;

public sealed class BoardProfileTests
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
        var expectedBoard = new Board
        {
            Name = boardCreation.Name,
            IsPinned = boardCreation.IsPinned,
            BoardSetting = new BoardSetting
            {
                Colour = boardCreation.BoardSetting.Colour
            },
            OwnerId = boardCreation.OwnerId
        };

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
        var expectedBoard = new Board
        {
            Id = boardEdition.Id,
            Name = boardEdition.Name,
            IsPinned = boardEdition.IsPinned,
            BoardSetting = new BoardSetting
            {
                Colour = boardEdition.BoardSetting.Colour
            }
        };

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
        var expectedBoardSetting = new BoardSetting
        {
            Colour = boardSettingCreation.Colour,
        };

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
        var expectedBoardSetting = new BoardSetting
        {
            Colour = boardSettingEdition.Colour
        };

        boardSetting
            .Should()
            .BeEquivalentTo(expectedBoardSetting);
    }
}
