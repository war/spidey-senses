using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SpiderControl.Api.Shared.Features.Spider.Commands;
using SpiderControl.Api.Shared.Features.Spider.Models;
using SpiderControl.Api.Shared.Testing.Infrastructure;
using SpiderControl.WebApiV2.Middleware;
using System.Net;
using System.Net.Http.Json;

namespace SpiderControl.WebApiV2.Tests.Controllers;

public class SpiderControllerIntegrationTests : TestBase<Program>
{
    public SpiderControllerIntegrationTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Theory]
    [InlineData("7 15", "2 4 Left", "FLFLFRFFLF", "3 1 Right")]
    [InlineData("5 5", "0 0 Up", "FFRFFFRRLF", "3 1 Down")]
    [InlineData("10 10", "5 5 Right", "FLFLFLFRFF", "3 5 Left")]
    public async Task Process_ValidInputs_ReturnsExpectedPosition(
        string wallInput,
        string spiderInput,
        string commandInput,
        string expectedPosition)
    {
        // Arrange
        var request = new ProcessSpiderCommand
        {
            WallInput = wallInput,
            SpiderInput = spiderInput,
            CommandInput = commandInput
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/spider/process", request);
        var result = await response.Content.ReadFromJsonAsync<ProcessSpiderCommandResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.FinalPosition.Should().Be(expectedPosition);
    }

    [Theory]
    [InlineData("", "2 4 Left", "FLFLFRFFLF", "Required fields cannot be empty")]
    [InlineData("7 15", "", "FLFLFRFFLF", "Required fields cannot be empty")]
    [InlineData("7 15", "2 4 Left", "", "Required fields cannot be empty")]
    public async Task Process_InvalidInputs_ReturnsBadRequest(
        string wallInput,
        string spiderInput,
        string commandInput,
        string expectedError)
    {
        // Arrange
        var request = new ProcessSpiderCommand
        {
            WallInput = wallInput,
            SpiderInput = spiderInput,
            CommandInput = commandInput
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/spider/process", request);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Should().NotBeNull();
        error!.Detail.Should().Contain(expectedError);
    }

    [Fact]
    public async Task Process_SpiderWouldFallOff_ReturnsBadRequest()
    {
        // Arrange
        var request = new ProcessSpiderCommand
        {
            WallInput = "5 5",
            SpiderInput = "0 0 Left",
            CommandInput = "F"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/spider/process", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Theory]
    [InlineData("/api/v1/spider/process")]
    [InlineData("/api/spider/process?api-version=1")]
    public async Task Process_DifferentVersionEndpoints_ReturnsOk(string endpoint)
    {
        // Arrange
        var request = new ProcessSpiderCommand
        {
            WallInput = "7 15",
            SpiderInput = "2 4 Left",
            CommandInput = "RR"
        };

        // Act
        var response = await Client.PostAsJsonAsync(endpoint, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.GetValues("api-version").Should().Equal("1");
    }

    [Theory]
    [InlineData("/api/v99/spider/process")]
    [InlineData("/api/v1/notspider/process")]
    [InlineData("/api/v1/spider/notprocess")]
    public async Task Process_InvalidVersion_ReturnsNotFound(string url)
    {
        // Arrange
        var request = new ProcessSpiderCommand
        {
            WallInput = "7 15",
            SpiderInput = "2 4 Left",
            CommandInput = "FLFLFRFFLF"
        };

        // Act
        var response = await Client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
