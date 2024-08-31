using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Application.Abstractions.Boards.Labels;
using Scrumboard.Domain.Boards.Labels;
using Scrumboard.Domain.Common;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Scrumboard.Web.Api.Boards.Labels;
using Scrumboard.Web.Api.Common.Profiles;
using Xunit;

namespace Scrumboard.Web.UnitTests.Api.Boards.Labels;

public sealed class LabelProfileTests
{
    private readonly IFixture _fixture;
   
    private readonly IMapper _mapper;

    public LabelProfileTests()
    {
        _fixture = new CustomizedFixture();
        
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
            cfg.AddProfile<LabelProfile>();
            cfg.SkipUserDtoMappings();
        });
        
        _mapper = mapperConfiguration.CreateMapper();
    }
    
    [Fact]
    public void Should_map_LabelCreationDto_to_LabelCreation()
    {
        // Arrange
        var labelCreationDto = _fixture.Create<LabelCreationDto>();
        
        // Act
        var labelCreation = _mapper.Map<LabelCreation>(labelCreationDto);
        
        // Assert
        var expectedLabelCreation = new LabelCreation
        {
            Name = labelCreationDto.Name,
            Colour = (Colour)labelCreationDto.Colour,
            BoardId = labelCreationDto.BoardId
        };

        labelCreation.Should()
            .BeEquivalentTo(expectedLabelCreation);
    }
    
    [Fact]
    public void Should_map_LabelEditionDto_to_LabelEdition()
    {
        // Arrange
        var labelEditionDto = _fixture.Create<LabelEditionDto>();
        
        // Act
        var labelEdition = _mapper.Map<LabelEdition>(labelEditionDto);
        
        // Assert
        var expectedLabelEdition = new LabelEdition
        {
            Id = labelEditionDto.Id,
            Name = labelEditionDto.Name,
            Colour = (Colour)labelEditionDto.Colour,
            BoardId = labelEditionDto.BoardId
        };

        labelEdition.Should()
            .BeEquivalentTo(expectedLabelEdition);
    }
    
    [Fact]
    public void Should_map_Label_to_LabelDto()
    {
        // Arrange
        var label = _fixture.Create<Label>();
        label.SetProperty(x => x.Name, _fixture.Create<string>());
        label.SetProperty(x => x.Colour, _fixture.Create<Colour>());
       
        // Act
        var labelDto = _mapper.Map<LabelDto>(label);
        
        // Assert
        var expectedLabelDto = new LabelDto
        {
            Id = label.Id.Value,
            Name = label.Name,
            Colour = label.Colour
        };

        labelDto.Should()
            .BeEquivalentTo(expectedLabelDto);
    }
}
