using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpiderControl.Api.Shared.Features.Spider.Commands;

namespace SpiderControl.WebApiV2.Features.Spider;

[ApiController]
[ApiVersion("1.0")]
[Route("api/spider")]
[Route("api/v{version:apiVersion}/spider")]
public class SpiderController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpiderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("process")]
    [ProducesResponseType(typeof(ProcessSpiderCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProcessSpiderCommandResponse>> Process(
        [FromBody] ProcessSpiderCommandRequest request,
        ApiVersion apiVersion,
        CancellationToken ct)
    {
        var command = new ProcessSpiderCommand(
            request.WallInput,
            request.SpiderInput,
            request.CommandInput);

        var result = await _mediator.Send(command, ct);
        Response.Headers.Append("api-version", apiVersion.ToString());
        return Ok(result);
    }
}
