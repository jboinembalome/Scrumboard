using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Application.Boards.Labels;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Shared.TestHelpers.Extensions;
using Xunit;

namespace Scrumboard.Application.UnitTests.Boards.Labels;

public sealed class LabelProfileTests
{
    private readonly IFixture _fixture;
   
    private readonly IMapper _mapper;
    
    public LabelProfileTests()
    {
        _fixture = new Fixture();
        
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<LabelProfile>();
        });
        
        _mapper = mapperConfiguration.CreateMapper();
    }
    
    [Fact]
    public void Should_map_LabelCreation_to_Label()
    {
        // Arrange
        var labelCreation = _fixture.Create<LabelCreation>();
        
        // Act
        var label = _mapper.Map<Label>(labelCreation);
        
        // Assert
        var expectedLabel = new Label(
            name: labelCreation.Name,
            colour: labelCreation.Colour,
            boardId: labelCreation.BoardId);

        label.Should()
            .BeEquivalentTo(expectedLabel);
    }
    
    [Fact]
    public void Should_map_LabelEdition_to_Label()
    {
        // Arrange
        var labelEdition = _fixture.Create<LabelEdition>();
        
        // Act
        var label = _mapper.Map<Label>(labelEdition);
        
        // Assert
        var expectedLabel = new Label(
            name: labelEdition.Name,
            colour: labelEdition.Colour,
            boardId: labelEdition.BoardId);
        
        expectedLabel.SetProperty(x => x.Id, label.Id);

        label.Should()
            .BeEquivalentTo(expectedLabel);
    }
}
