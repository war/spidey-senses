using Microsoft.AspNetCore.Mvc.Testing;
using SpiderControl.Api.Shared.Features.Spider.Commands;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using SpiderControl.Api.Shared.Testing.Infrastructure;

namespace SpiderControl.WebApiV2.Tests.Controllers;

public class SpiderControllerTests : TestBase<Program>
{
    public SpiderControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task ProcessCommands_ValidInput_ReturnsExpectedPosition()
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
}
