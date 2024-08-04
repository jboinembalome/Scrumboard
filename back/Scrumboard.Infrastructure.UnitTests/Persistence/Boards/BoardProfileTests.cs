using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Infrastructure.Persistence.Boards;
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
        });
        
        _mapper = mapperConfiguration.CreateMapper();
    }
    
    [Fact]
    public void Should_map_BoardCreation_to_BoardDao()
    {
        // Arrange
        var boardCreation = _fixture.Create<BoardCreation>();
        
        // Act
        var boardDao = _mapper.Map<BoardDao>(boardCreation);
        
        // Assert
        var expectedBoardDao = new BoardDao
        {
            Name = boardCreation.Name,
            IsPinned = boardCreation.IsPinned,
            BoardSetting = new BoardSettingDao
            {
                Colour = boardCreation.BoardSetting.Colour
            }
        };

        boardDao
            .Should()
            .BeEquivalentTo(expectedBoardDao);
    }
    
    [Fact]
    public void Should_map_BoardEdition_to_BoardDao()
    {
        // Arrange
        var boardEdition = _fixture.Create<BoardEdition>();
        
        // Act
        var boardDao = _mapper.Map<BoardDao>(boardEdition);
        
        // Assert
        var expectedBoardDao = new BoardDao
        {
            Id = boardEdition.Id,
            Name = boardEdition.Name,
            IsPinned = boardEdition.IsPinned,
            BoardSetting = new BoardSettingDao
            {
                Id = boardEdition.BoardSetting.Id,
                BoardId = boardEdition.BoardSetting.BoardId,
                Colour = boardEdition.BoardSetting.Colour,
                Subscribed = boardEdition.BoardSetting.Subscribed,
                CardCoverImage = boardEdition.BoardSetting.CardCoverImage
            }
        };

        boardDao
            .Should()
            .BeEquivalentTo(expectedBoardDao);
    }
    
    [Fact]
    public void Should_map_BoardSettingCreation_to_BoardSettingDao()
    {
        // Arrange
        var boardSettingCreation = _fixture.Create<BoardSettingCreation>();
        
        // Act
        var boardSettingDao = _mapper.Map<BoardSettingDao>(boardSettingCreation);
        
        // Assert
        var expectedBoardSettingDao = new BoardSettingDao
        {
            Colour = boardSettingCreation.Colour,
        };

        boardSettingDao
            .Should()
            .BeEquivalentTo(expectedBoardSettingDao);
    }
    
    [Fact]
    public void Should_map_BoardSetting_to_BoardSettingDao()
    {
        // Arrange
        var boardSetting = _fixture.Create<BoardSetting>();
        
        // Act
        var boardSettingDao = _mapper.Map<BoardSettingDao>(boardSetting);
        
        // Assert
        var expectedBoardSettingDao = new BoardSettingDao
        {
            Id = boardSetting.Id,
            BoardId = boardSetting.BoardId,
            Colour = boardSetting.Colour,
            Subscribed = boardSetting.Subscribed,
            CardCoverImage = boardSetting.CardCoverImage
        };

        boardSettingDao
            .Should()
            .BeEquivalentTo(expectedBoardSettingDao);
    }
    
    [Fact]
    public void Should_map_BoardDao_to_Board()
    {
        // Arrange
        var boardDao = _fixture.Create<BoardDao>();
        
        // Act
        var board = _mapper.Map<Board>(boardDao);
        
        // Assert
        var expectedBoard = new Board
        {
            Id = (BoardId)boardDao.Id,
            Name = boardDao.Name,
            IsPinned = boardDao.IsPinned,
            Uri = boardDao.Uri,
            BoardSetting = new BoardSetting
            {
                Id = (BoardSettingId)boardDao.BoardSetting.Id,
                BoardId = (BoardId)boardDao.BoardSetting.BoardId,
                Colour = boardDao.BoardSetting.Colour,
                Subscribed = boardDao.BoardSetting.Subscribed,
                CardCoverImage = boardDao.BoardSetting.CardCoverImage
            },
            CreatedBy = (UserId)boardDao.CreatedBy,
            CreatedDate = boardDao.CreatedDate,
            LastModifiedBy = (UserId)boardDao.LastModifiedBy!,
            LastModifiedDate = boardDao.LastModifiedDate
        };

        board
            .Should()
            .BeEquivalentTo(expectedBoard);
    }
    
    [Fact]
    public void Should_map_BoardSettingDao_to_BoardSetting()
    {
        // Arrange
        var boardSettingDao = _fixture.Create<BoardSettingDao>();
        
        // Act
        var boardSetting = _mapper.Map<BoardSetting>(boardSettingDao);
        
        // Assert
        var expectedBoardSetting = new BoardSetting
        {
            Id = (BoardSettingId)boardSettingDao.Id,
            BoardId = (BoardId)boardSettingDao.BoardId,
            Colour = boardSettingDao.Colour,
            Subscribed = boardSettingDao.Subscribed,
            CardCoverImage = boardSettingDao.CardCoverImage
        };

        boardSetting
            .Should()
            .BeEquivalentTo(expectedBoardSetting);
    }
}
