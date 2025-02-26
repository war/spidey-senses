using Microsoft.AspNetCore.Mvc;
using SpiderControl.Api.Shared.Features.Api.Models;
using System.Diagnostics;
using System.Reflection;

namespace SpiderControl.WebApiV2.Features.Api;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private static readonly Stopwatch _uptime = Stopwatch.StartNew();

    public ApiController(ILogger<ApiController> logger, IWebHostEnvironment environment)
    {
        _environment = environment;
        _ = _uptime.Elapsed;
    }

    [HttpGet("version")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        var buildDate = System.IO.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location);

        var versionString = version?.ToString() ?? "1.0.0";
        var environmentString = _environment.EnvironmentName;
        var buildDateString = buildDate.ToString("o");

        var result = new VersionResponse(versionString, environmentString, buildDateString);

        return Ok(result);
    }

    [HttpGet("metrics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetMetrics()
    {
        var uptime = _uptime.Elapsed.ToString();
        var memoryUsage = Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024) + " MB";
        var cpuUsage = Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds;
        var threadCount = Process.GetCurrentProcess().Threads.Count;
        var requestCount = 0; // TODO: figure out how to track this

        var result = new MetricsResponse(uptime, memoryUsage, cpuUsage, threadCount, requestCount);

        return Ok(result);
    }

    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetStatus()
    {
        var apiComponent = new ComponentStatus("API", "Available", "Running normally");
        var dbComponent = new ComponentStatus("FakeDatabase", "Available", "Connected");
        var cacheComponent = new ComponentStatus("FakeCache", "Available", "Connected");

        var componentArray = new ComponentStatus[]
        {
            apiComponent,
            dbComponent,
            cacheComponent
        };

        var uptimeStatus = _uptime.Elapsed.ToString();

        var statusResponse = new StatusResponse("Available", componentArray, uptimeStatus);

        return Ok(statusResponse);
    }

    [HttpGet("ping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Ping()
    {
        return Ok("pong");
    }

    [HttpGet("errors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetErrorCatalog()
    {
        var wallError = new ErrorInfo("WALL-001", "Invalid wall dimensions", 400);
        var spiderError = new ErrorInfo("SPIDER-001", "Invalid spider position", 400);
        var commandError = new ErrorInfo("COMMAND-001", "Invalid command", 400);
        var moveError = new ErrorInfo("MOVE-001", "spider would fall off wall", 422);

        var errors = new ErrorInfo[]
        {
            wallError,
            spiderError,
            commandError,
            moveError
        };

        var result = new ErrorCatalogResponse(errors);

        return Ok(result);
    }

    [HttpGet("config")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetPublicConfig()
    {
        //TODO: pull these from config
        var apiRateLimit = 100;
        var maxCommandLength = 1000;
        var validCommands = new[] { 'F', 'L', 'R' };
        var maxWallDimensions = new { Width = 1000, Height = 1000 };

        var result = new ConfigResponse(apiRateLimit, maxCommandLength, validCommands, maxWallDimensions);

        return Ok(result);
    }
}