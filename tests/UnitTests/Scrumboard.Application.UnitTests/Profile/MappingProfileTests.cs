using AutoMapper;
using FluentAssertions;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Profiles;
using System;
using System.Runtime.Serialization;
using Scrumboard.Application.Boards.CreateBoard;
using Scrumboard.Domain.Boards;
using Xunit;

namespace Scrumboard.Application.UnitTests.Profile
{
    public class MappingProfileTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
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
        [InlineData(typeof(Board), typeof(CreateBoardCommand))]
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
                return Activator.CreateInstance(type);

            // Type without parameterless constructor
            return FormatterServices.GetUninitializedObject(type);
        }
    }

}
