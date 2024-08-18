using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Infrastructure.Abstractions.Persistence.Boards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Web.Api.Boards;
using Xunit;

namespace Scrumboard.Web.UnitTests.Api.Boards;

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
            cfg.SkipUserDtoMappings();
        });
        
        _mapper = mapperConfiguration.CreateMapper();
    }
    
    [Fact]
    public void Should_map_BoardEditionDto_to_BoardEdition()
    {
        // Arrange
        var boardEditionDto = _fixture.Create<BoardEditionDto>();
        
        // Act
        var boardEdition = _mapper.Map<BoardEdition>(boardEditionDto);
        
        // Assert
        var expectedBoardEdition = new BoardEdition
        {
            Id = (BoardId)boardEditionDto.Id,
            Name = boardEditionDto.Name,
            IsPinned = boardEditionDto.IsPinned,
            BoardSetting = new BoardSettingEdition
            {
                Colour = Colour.From(boardEditionDto.BoardSetting.Colour)
            }
        };

        boardEdition.Should()
            .BeEquivalentTo(expectedBoardEdition);
    }
    
    [Fact]
    public void Should_map_BoardCreationDto_to_BoardCreation()
    {
        // Arrange
        var boardCreationDto = _fixture.Create<BoardCreationDto>();
        
        // Act
        var boardCreation = _mapper.Map<BoardCreation>(boardCreationDto);
        
        // Assert
        var expectedBoardCreation = new BoardCreation
        {
            Name = boardCreationDto.Name,
            IsPinned = boardCreationDto.IsPinned,
            BoardSetting = new BoardSettingCreation
            {
                Colour = Colour.From(boardCreationDto.BoardSetting.Colour)
            }
        };

        boardCreation.Should()
            .BeEquivalentTo(expectedBoardCreation);
    }
    
    [Fact]
    public void Should_map_BoardSettingCreationDto_to_BoardSettingCreation()
    {
        // Arrange
        var boardSettingCreationDto = _fixture.Create<BoardSettingCreationDto>();
        
        // Act
        var boardSettingCreation = _mapper.Map<BoardSettingCreation>(boardSettingCreationDto);
        
        // Assert
        var expectedBoardSettingCreation = new BoardSettingCreation
        {
            Colour = Colour.From(boardSettingCreationDto.Colour)
        };

        boardSettingCreation.Should()
            .BeEquivalentTo(expectedBoardSettingCreation);
    }
    
    [Fact]
    public void Should_map_BoardSettingEditionDto_to_BoardSettingEdition()
    {
        // Arrange
        var boardSettingEditionDto = _fixture.Create<BoardSettingEditionDto>();
        
        // Act
        var boardSettingEdition = _mapper.Map<BoardSettingEdition>(boardSettingEditionDto);
        
        // Assert
        var expectedBoardSettingEdition = new BoardSettingEdition
        {
            Colour = Colour.From(boardSettingEditionDto.Colour)
        };

        boardSettingEdition.Should()
            .BeEquivalentTo(expectedBoardSettingEdition);
    }
    
    [Fact]
    public void Should_map_Board_to_BoardDto()
    {
        // Arrange
        var board = _fixture.Create<Board>();
        board.SetProperty(x => x.Name, _fixture.Create<string>());
        board.SetProperty(x => x.BoardSetting, _fixture.Create<BoardSetting>());
        
        // Act
        var boardDto = _mapper.Map<BoardDto>(board);
        
        // Assert
        var expectedBoardDto = new BoardDto
        {
            Id = board.Id.Value,
            Name = board.Name,
            IsPinned = board.IsPinned,
            BoardSetting = new BoardSettingDto
            {
                Id = board.BoardSetting.Id.Value,
                Colour = board.BoardSetting.Colour
            },
            CreatedDate = board.CreatedDate,
            LastModifiedDate = board.LastModifiedDate
        };

        boardDto.Should()
            .BeEquivalentTo(expectedBoardDto);
    }
    
    [Fact]
    public void Should_map_BoardSetting_to_BoardSettingDto()
    {
        // Arrange
        var boardSetting = _fixture.Create<BoardSetting>();
        
        // Act
        var boardSettingDto = _mapper.Map<BoardSettingDto>(boardSetting);
        
        // Assert
        var expectedBoardSettingDto = new BoardSettingDto
        {
            Id = boardSetting.Id.Value,
            Colour = boardSetting.Colour
        };

        boardSettingDto.Should()
            .BeEquivalentTo(expectedBoardSettingDto);
    }
}
