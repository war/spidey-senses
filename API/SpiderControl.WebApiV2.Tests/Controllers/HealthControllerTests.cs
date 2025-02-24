using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SpiderControl.Api.Shared.Testing.Infrastructure;

namespace SpiderControl.WebApiV2.Tests.Features.Health;

public class HealthControllerTests : TestBase<Program>
{
    public HealthControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task Check_ReturnsOk()
    {
        // Act
        var response = await Client.GetAsync("/api/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
