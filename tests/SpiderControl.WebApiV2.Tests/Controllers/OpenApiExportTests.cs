// tests/SpiderControl.WebApiV2.Tests/OpenApiExportTests.cs
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SpiderControl.Api.Shared.Testing.Infrastructure;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SpiderControl.WebApiV2.Tests
{
    public class OpenApiExportTests : TestBase<Program>
    {
        public OpenApiExportTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task ExportOpenApiDocument_ShouldGenerateValidJson()
        {
            // Arrange
            var response = await Client.GetAsync("/swagger/v1/swagger.json");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            json.Should().NotBeNullOrEmpty("OpenAPI JSON should not be empty");

            // Create output directory
            var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "openapi-output");
            Directory.CreateDirectory(outputDir);

            // Save to file
            var filePath = Path.Combine(outputDir, "openapi.json");
            File.WriteAllText(filePath, json);

            // Assert
            File.Exists(filePath).Should().BeTrue("OpenAPI JSON file should be created");
            json.Should().Contain("Spider Control API", "JSON should contain API title");
        }
    }
}