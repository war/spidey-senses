using Microsoft.AspNetCore.Mvc.Testing;
using SpiderControl.Api.Shared.Features.Spider.Commands;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using SpiderControl.Api.Shared.Testing.Infrastructure;

namespace SpiderControl.WebApiV2.Tests;

public class ApiVersioningTests : TestBase<Program>
{
    public ApiVersioningTests(WebApplicationFactory<Program> factory) : base(factory) {}

    [Theory]
    [InlineData("/api/v1/spider/process")]
    [InlineData("/api/v2/spider/process")]
    public async Task Process_DifferentVersions_ReturnsCorrectStatusCode(string endpoint)
    {
        // Arrange
        var request = new ProcessSpiderCommandRequest
        {
            WallInput = "7 15",
            SpiderInput = "2 4 Left",
            CommandInput = "FLFLFRFFLF"
        };

        // Act
        var response = await Client.PostAsJsonAsync(endpoint, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Process_UsingVersionHeader_ReturnsCorrectVersion()
    {
        // Arrange
        var request = new ProcessSpiderCommandRequest
        {
            WallInput = "7 15",
            SpiderInput = "2 4 Left",
            CommandInput = "FLFLFRFFLF"
        };
        
        Client.DefaultRequestHeaders.Add("x-api-version", "2.0");

        // Act
        var response = await Client.PostAsJsonAsync("/api/spider/process", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.GetValues("api-version").Should().Contain("2.0");
    }

    [Fact]
    public async Task Process_UsingQueryString_ReturnsCorrectVersion()
    {
        // Arrange
        var request = new ProcessSpiderCommandRequest
        {
            WallInput = "7 15",
            SpiderInput = "2 4 Left",
            CommandInput = "FLFLFRFFLF"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/spider/process?api-version=2.0", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.GetValues("api-version").Should().Contain("2.0");
    }

    [Fact]
    public async Task Process_InvalidVersion_ReturnsVersionNegotiationError()
    {
        // Arrange
        var request = new ProcessSpiderCommandRequest
        {
            WallInput = "7 15",
            SpiderInput = "2 4 Left",
            CommandInput = "FLFLFRFFLF"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v5/spider/process", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("version");
    }
}
