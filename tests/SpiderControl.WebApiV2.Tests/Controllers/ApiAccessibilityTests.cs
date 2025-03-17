using Microsoft.AspNetCore.Mvc.Testing;
using SpiderControl.Api.Shared.Testing.Infrastructure;

namespace SpiderControl.WebApiV2.Tests.Controllers;

public class ApiAccessibilityTests : TestBase<Program>
{
    public ApiAccessibilityTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Theory]
    [MemberData(nameof(GetApiEndpoints))]
    public async Task ApiEndpoint_IsAccessible(string endpoint, HttpMethod method, int expectedStatusCode)
    {
        // Arrange
        var request = new HttpRequestMessage(method, endpoint);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.Equal(expectedStatusCode, (int)response.StatusCode);
    }

    public static IEnumerable<object[]> GetApiEndpoints()
    {
        // TODO: get endpoints from swagger.json
        yield return new object[] { "/api/health/liveness", HttpMethod.Get, 200 };
        yield return new object[] { "/api/health/check", HttpMethod.Get, 200 };
        yield return new object[] { "/api/health/readiness", HttpMethod.Get, 200 };
        yield return new object[] { "/api/version", HttpMethod.Get, 200 };
        yield return new object[] { "/api/metrics", HttpMethod.Get, 200 };
        yield return new object[] { "/api/status", HttpMethod.Get, 200 };
        yield return new object[] { "/api/ping", HttpMethod.Get, 200 };
        yield return new object[] { "/api/errors", HttpMethod.Get, 200 };
        yield return new object[] { "/api/config", HttpMethod.Get, 200 };
    }
}
