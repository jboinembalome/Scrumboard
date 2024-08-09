using AutoMapper;
using Scrumboard.Web.Api.Users;

namespace Scrumboard.Web.UnitTests;

internal static class MapperConfigurationExpressionExtensions
{
    internal static void SkipUserDtoMappings(this IMapperConfigurationExpression config)
    {
        // Skip `UserId => USerDto` mappings as we don't have the logic here
        var userDtoClass = typeof(UserDto);

        config.ShouldMapProperty = p =>
        {
            var isUserDto = p.PropertyType.IsAssignableTo(userDtoClass);

            return isUserDto == false;
        };
    }
}
