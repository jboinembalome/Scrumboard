using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Application.Abstractions.ListBoards;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.ListBoards;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.Web.Api.ListBoards;
using Xunit;

namespace Scrumboard.Web.UnitTests.Api.ListBoards;

public sealed class ListBoardProfileTests
{
    private readonly IFixture _fixture;
   
    private readonly IMapper _mapper;
    
    public ListBoardProfileTests()
    {
        _fixture = new CustomizedFixture();
        
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ListBoardProfile>();
            cfg.SkipUserDtoMappings();
        });
        
        _mapper = mapperConfiguration.CreateMapper();
    }
    
    [Fact]
    public void Should_map_ListBoardCreationDto_to_ListBoardCreation()
    {
        // Arrange
        var listBoardCreationDto = _fixture.Create<ListBoardCreationDto>();
        
        // Act
        var listBoardCreation = _mapper.Map<ListBoardCreation>(listBoardCreationDto);
        
        // Assert
        var expectedListBoardCreation = new ListBoardCreation
        {
            Name = listBoardCreationDto.Name,
            Position = listBoardCreationDto.Position,
            BoardId = listBoardCreationDto.BoardId
        };

        listBoardCreation.Should()
            .BeEquivalentTo(expectedListBoardCreation);
    }
    
    [Fact]
    public void Should_map_ListBoardEditionDto_to_ListBoardEdition()
    {
        // Arrange
        var listBoardEditionDto = _fixture.Create<ListBoardEditionDto>();
        
        // Act
        var listBoardEdition = _mapper.Map<ListBoardEdition>(listBoardEditionDto);
        
        // Assert
        var expectedListBoardEdition = new ListBoardEdition
        {
            Id = listBoardEditionDto.Id,
            Name = listBoardEditionDto.Name,
            Position = listBoardEditionDto.Position,
            BoardId = listBoardEditionDto.BoardId
        };

        listBoardEdition.Should()
            .BeEquivalentTo(expectedListBoardEdition);
    }
    
    [Fact]
    public void Should_map_ListBoard_to_ListBoardDto()
    {
        // Arrange
        var listBoard = _fixture.Create<ListBoard>();
        listBoard.SetProperty(x => x.Name, _fixture.Create<string>());
        listBoard.SetProperty(x => x.Position, _fixture.Create<int>());
        listBoard.SetProperty(x => x.BoardId, _fixture.Create<BoardId>());
       
        // Act
        var listBoardDto = _mapper.Map<ListBoardDto>(listBoard);
        
        // Assert
        var expectedListBoardDto = new ListBoardDto
        {
            Id = listBoard.Id.Value,
            Name = listBoard.Name,
            Position = listBoard.Position
        };

        listBoardDto.Should()
            .BeEquivalentTo(expectedListBoardDto);
    }
}
