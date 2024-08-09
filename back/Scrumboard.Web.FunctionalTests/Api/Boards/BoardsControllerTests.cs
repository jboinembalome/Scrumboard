using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Scrumboard.Web.Api.Boards;
using Scrumboard.Web.FunctionalTests.Utilities;
using Xunit;

namespace Scrumboard.Web.FunctionalTests.Api.Boards;

public class BoardsControllerTests : IClassFixture<CustomWebApplicationFactoryFixture<Startup>>
{
    private readonly CustomWebApplicationFactoryFixture<Startup> _factory;

    public BoardsControllerTests(CustomWebApplicationFactoryFixture<Startup> factory)
    {
        _factory = factory;
    }

    [Theory(Skip = "TODO: Update test when authentication migration is done")]
    [InlineData("/api/boards")]
    [InlineData("/api/boards/1")]
    public async Task Get_EndpointsReturnUnauthorizedToAnonymousUserForRestrictedUrls(string url)
    {
        // Arrange
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact(Skip = "TODO: Update test when authentication migration is done")]
    public async Task GetBoards_ReturnsSuccessResult()
    {
        // Arrange
        var provider = TestClaimsProvider.WithAdherentClaims();
        var client = _factory.CreateClientWithTestAuth(provider);

        // Act
        var response = await client.GetAsync("/api/boards");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<IEnumerable<BoardDto>>(responseString);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeAssignableTo<IEnumerable<BoardDto>>();
        result.Should().NotBeEmpty();
    }

    [Fact(Skip = "TODO: Update test when authentication migration is done")]
    public async Task GetBoardDetail_ReturnsSuccessResult()
    {
        // Arrange
        var provider = TestClaimsProvider.WithAdherentClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        var id = 1;

        // Act
        var response = await client.GetAsync($"/api/boards/{id}");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<BoardDto>(responseString);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeOfType<BoardDto>();
        result.Should().NotBeNull();
    }

    [Fact(Skip = "TODO: Update test when authentication migration is done")]
    public async Task Get_ReturnsNotFoundGivenInvalidId()
    {
        // Arrange
        var provider = TestClaimsProvider.WithAdherentClaims();
        var client = _factory.CreateClientWithTestAuth(provider);
        var id = 0;

        // Act
        var response = await client.GetAsync($"/api/boards/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
