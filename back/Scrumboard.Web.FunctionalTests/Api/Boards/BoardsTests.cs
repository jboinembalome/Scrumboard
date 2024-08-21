using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Common;
using Scrumboard.Web.Api.Boards;
using Scrumboard.Web.FunctionalTests.Utilities;
using Xunit;

namespace Scrumboard.Web.FunctionalTests.Api.Boards;

public sealed class BoardsTests(
    CustomWebApplicationFactory factory) : FunctionalTestsBase(factory)
{
    [Theory]
    [InlineData("/api/boards")]
    [InlineData("/api/boards/1")]
    public async Task Get_endpoints_should_return_Unauthorized_when_anonymous_user(string url)
    {
        // Arrange
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_boards_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);

        // Act
        var response = await client.GetAsync("/api/boards");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<IReadOnlyCollection<BoardDto>>(responseString);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeAssignableTo<IEnumerable<BoardDto>>();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Get_board_by_id_should_return_Ok()
    {
        // Arrange
        var client = _factory.CreateUserClient(SeededUser.Adherent);

        var board = await Given_a_board();
        
        // Act
        var response = await client.GetAsync($"/api/boards/{board.Id}");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<BoardDto>(responseString);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeOfType<BoardDto>();
        result.Should().NotBeNull();
    }

    private async Task<Board> Given_a_board()
        => await _factory.AddEntityAsync(new Board(
            name: "New Board",
            isPinned: false,
            boardSetting: new BoardSetting { Colour = Colour.Blue },
            ownerId: SeededUser.Adherent.Id));
}
