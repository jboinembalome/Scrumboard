using System.Reflection;
using System.Runtime.CompilerServices;
using AutoMapper;
using FluentAssertions;
using Scrumboard.Application.Boards.Commands.CreateBoard;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Domain.Boards;
using Xunit;

namespace Scrumboard.Application.UnitTests.Profile;

public class MappingProfileTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        _configuration = new MapperConfiguration(config =>
            config.AddMaps(Assembly.GetAssembly(typeof(ApplicationServiceRegistration))));

        _mapper = _configuration.CreateMapper();
    }

    [Fact(Skip = "TODO: Update this test")]
    public void AssertConfigurationIsValid_HaveValidConfiguration()
    {
        // Act
        Action action = () => _configuration.AssertConfigurationIsValid();

        // Assert
        action.Should().NotThrow();
    }

    [Theory]
    [InlineData(typeof(Board), typeof(BoardDto))]
    [InlineData(typeof(Board), typeof(BoardDetailDto))]
    [InlineData(typeof(CreateBoardCommand), typeof(Board))]
    public void Map_SupportMappingFromSourceToDestination(Type source, Type destination)
    {
        // Arrange
        var instance = GetInstanceOf(source);

        // Act
        Action action = () => _mapper.Map(instance, source, destination);

        // Assert
        action.Should().NotThrow();
    }

    private static object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return RuntimeHelpers.GetUninitializedObject(type);
    }
}
