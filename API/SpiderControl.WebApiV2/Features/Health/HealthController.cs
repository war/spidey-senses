using Microsoft.AspNetCore.Mvc;

namespace SpiderControl.WebApiV2.Features.Health;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public ActionResult Check()
    {
        return Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
    }
}
