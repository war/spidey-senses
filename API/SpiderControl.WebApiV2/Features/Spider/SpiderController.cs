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
    private readonly ILogger<SpiderController> _logger;

    public SpiderController(IMediator mediator, ILogger<SpiderController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("process")]
    [ProducesResponseType(typeof(ProcessSpiderCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProcessSpiderCommandResponse>> Process(
        [FromBody] ProcessSpiderCommandRequest request,
        ApiVersion apiVersion,
        CancellationToken ct)
    {
        _logger.LogInformation("Received spider command request");

        var command = new ProcessSpiderCommand
        {
            WallInput = request.WallInput,
            SpiderInput = request.SpiderInput,
            CommandInput = request.CommandInput
        };

        var result = await _mediator.Send(command, ct);
        Response.Headers.Append("api-version", apiVersion.ToString());
        return Ok(result);
    }
}
