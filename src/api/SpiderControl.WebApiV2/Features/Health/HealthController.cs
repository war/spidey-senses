using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SpiderControl.WebApiV2.Features.Health;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public HealthController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [HttpGet("check")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult> Check()
    {
        var report = await _healthCheckService.CheckHealthAsync();

        var isHealthy = report.Status == HealthStatus.Healthy;

        var healthyResult = Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
        var unhealthyResult = StatusCode(StatusCodes.Status503ServiceUnavailable,
                new { Status = "Unhealthy", Timestamp = DateTime.UtcNow });

        return isHealthy ? healthyResult : unhealthyResult;
    }

    [HttpGet("liveness")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Liveness()
    {
        return Ok(new { Status = "Alive", Timestamp = DateTime.UtcNow });
    }

    [HttpGet("readiness")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult> Readiness()
    {
        var report = await _healthCheckService.CheckHealthAsync();

        var isHealthy = report.Status == HealthStatus.Healthy;

        var healthyResult = Ok(new { Status = "Ready", Timestamp = DateTime.UtcNow });
        var unhealthyResult = StatusCode(StatusCodes.Status503ServiceUnavailable,
                new { Status = "Not Ready", Timestamp = DateTime.UtcNow });

        return isHealthy ? healthyResult : unhealthyResult;
    }
}
