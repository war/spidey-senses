using Microsoft.AspNetCore.Mvc;
using SpiderControl.Api.Shared.Features.Spider.Commands;

namespace SpiderControl.WebApiV2.Features.Spider;

[ApiController]
[ApiVersion("1.0")]
[Route("api/spider")]
[Route("api/v{version:apiVersion}/spider")]
public class SpiderController : ControllerBase
{
    [HttpPost("process")]
    public ActionResult<ProcessSpiderCommandResponse> Process([FromBody] ProcessSpiderCommandRequest request)
    {
        return Ok(new ProcessSpiderCommandResponse { FinalPosition = string.Empty });
    }
}
