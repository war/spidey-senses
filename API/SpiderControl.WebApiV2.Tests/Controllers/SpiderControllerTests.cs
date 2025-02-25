using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SpiderControl.Api.Shared.Features.Spider.Commands;
using SpiderControl.Api.Shared.Testing.Infrastructure;
using SpiderControl.WebApiV2.Middleware;

namespace SpiderControl.WebApiV2.Tests.Controllers;

public class SpiderControllerTests : TestBase<Program>
{
    public SpiderControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task ProcessCommands_ValidInput_Returns200Ok()
    {
        // Arrange
        var request = new ProcessSpiderCommandRequest
        {
            WallInput = "7 15",
            SpiderInput = "2 4 Left",
            CommandInput = "FLFLFRFFLF"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/spider/process", request);
        var result = await response.Content.ReadFromJsonAsync<ProcessSpiderCommandResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.FinalPosition.Should().Be("3 1 Right");
    }

    [Fact]
    public async Task ProcessCommands_MissingFields_Returns400BadRequest()
    {
        // Arrange
        var request = new ProcessSpiderCommandRequest
        {
            WallInput = "",
            SpiderInput = "2 4 Left",
            CommandInput = "FLFLFRFFLF"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/spider/process", request);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Should().NotBeNull();
        error!.Title.Should().Be("Invalid Request");
    }

    [Fact]
    public async Task ProcessCommands_InvalidFormat_Returns400BadRequest()
    {
        // Arrange
        var request = new ProcessSpiderCommandRequest
        {
            WallInput = "invalid",
            SpiderInput = "2 4 Left",
            CommandInput = "FLFLFRFFLF"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/spider/process", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ProcessCommands_InvalidPosition_Returns422UnprocessableEntity()
    {
        // Arrange
        var request = new ProcessSpiderCommandRequest
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

    [Fact]
    public async Task GetSpider_InvalidId_Returns404NotFound()
    {
        // Act
        var response = await Client.GetAsync("/api/v1/spider/nonexistent");
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        error.Should().NotBeNull();
        error!.Title.Should().Be("Not Found");
    }
}
