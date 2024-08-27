using AutoFixture;
using FluentAssertions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Shared.TestHelpers.Extensions;
using Scrumboard.Shared.TestHelpers.Fixtures;
using Xunit;

namespace Scrumboard.Domain.UnitTests.Boards;

public sealed class BoardSettingTests
{
    private readonly IFixture _fixture = new CustomizedFixture();
    
    [Fact]
    public void Update_should_only_set_Colour()
    {
        // Arrange
        var boardSetting = _fixture.Create<BoardSetting>();
        
        var newColour = _fixture.Create<Colour>();

        // Act
        boardSetting.Update(newColour);

        // Assert
        var expectedBoardSetting = new BoardSetting();
        expectedBoardSetting.SetProperty(x => x.Id, boardSetting.Id);
        expectedBoardSetting.SetProperty(x => x.BoardId, boardSetting.BoardId);
        expectedBoardSetting.SetProperty(x => x.Colour, newColour);
        
        boardSetting.Should()
            .BeEquivalentTo(expectedBoardSetting);
    }
}
