using FluentAssertions;
using Moq;

namespace Scrumboard.Shared.TestHelpers.Moq;

/// <summary>
/// Provides custom matchers for Moq verification methods.
/// </summary>
/// <remark>
/// This class is necessary because we cannot add an extension method into the <see cref="It"/> static class.
/// Instead, we create a separate class to provide our custom matchers, like <see cref="IsEquivalentTo{T}"/>.
/// </remark>
public static class CustomIt
{
    public static T IsEquivalentTo<T>(T expected) where T : class 
        => Match.Create<T>(actual =>
        {
            actual.Should().BeEquivalentTo(expected);
            return true;
        });
}
