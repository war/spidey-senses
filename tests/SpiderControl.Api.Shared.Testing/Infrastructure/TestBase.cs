using Microsoft.AspNetCore.Mvc.Testing;
using SpiderControl.Api.Shared.Features.Spider.Commands;
using Xunit;

namespace SpiderControl.Api.Shared.Testing.Infrastructure;

public abstract class TestBase<TProgram> : IClassFixture<WebApplicationFactory<TProgram>> where TProgram : class
{
    protected readonly HttpClient Client;
    protected readonly WebApplicationFactory<TProgram> Factory;

    protected TestBase(WebApplicationFactory<TProgram> factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    protected static ProcessSpiderCommandRequest CreateDefaultRequest() => new()
    {
        WallInput = "7 15",
        SpiderInput = "2 4 Left",
        CommandInput = "FLFLFRFFLF"
    };
}
