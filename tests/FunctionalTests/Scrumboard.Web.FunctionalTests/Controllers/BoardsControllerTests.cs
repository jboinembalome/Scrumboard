using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Features.Boards.Commands.CreateBoard;
using Scrumboard.Application.Features.Boards.Commands.UpdateBoard;
using Scrumboard.Web.FunctionalTests.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Scrumboard.Web.FunctionalTests.Controllers
{
    public class BoardsControllerTests : IClassFixture<CustomWebApplicationFactoryFixture<Startup>>
    {
        private readonly CustomWebApplicationFactoryFixture<Startup> _factory;

        public BoardsControllerTests(CustomWebApplicationFactoryFixture<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
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

        [Fact]
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

        [Fact]
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
            var result = JsonConvert.DeserializeObject<BoardDetailDto>(responseString);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().BeOfType<BoardDetailDto>();
            result.Should().NotBeNull();
        }

        [Fact]
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

        [Fact]
        public async Task CreateBoard_ReturnsSuccessResult()
        {
            // Arrange
            var provider = TestClaimsProvider.WithAdherentClaims();
            var client = _factory.CreateClientWithTestAuth(provider);

            // Act
            var response = await client.PostAsync("api/boards", null);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var board = JsonConvert.DeserializeObject<CreateBoardCommandResponse>(responseString);

            // Assert
            board.Board.Should().NotBeNull();
            board.Board.Id.Should().BePositive();
            board.Board.Name.Should().Be("Untitled Board");
        }

        [Fact]
        public async Task UpdateBoard_ReturnsNoContent()
        {
            // Arrange
            var provider = TestClaimsProvider.WithAdherentClaims();
            var client = _factory.CreateClientWithTestAuth(provider);
            var id = 2;
            var request = new UpdateBoardCommand()
            {
                BoardId = id,
                Name = "Updated board name",
                Uri= "updated-board-name"
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"api/boards/{id}", jsonContent);
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateBoard_ReturnsBadRequest()
        {
            // Arrange
            var provider = TestClaimsProvider.WithAdherentClaims();
            var client = _factory.CreateClientWithTestAuth(provider);
            var id = 2;
            var badId = 1;
            var request = new UpdateBoardCommand()
            {
                BoardId = badId,
                Name = "Updated board name"
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"api/boards/{id}", jsonContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteBoard_ReturnsNoContent()
        {
            // Arrange
            var provider = TestClaimsProvider.WithAdherentClaims();
            var client = _factory.CreateClientWithTestAuth(provider);
            var id = 3;

            // Act
            var response = await client.DeleteAsync($"api/boards/{id}");
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteBoard_ReturnsNotFoundGivenInvalidId()
        {
            // Arrange
            var provider = TestClaimsProvider.WithAdherentClaims();
            var client = _factory.CreateClientWithTestAuth(provider);
            var id = 0;

            // Act
            var response = await client.DeleteAsync($"api/boards/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
