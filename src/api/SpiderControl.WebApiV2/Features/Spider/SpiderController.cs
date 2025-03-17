using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpiderControl.Api.Shared.Features.Spider.Commands;
using SpiderControl.Api.Shared.Features.Spider.Models;

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
    [ProducesResponseType(typeof(SpiderProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SpiderProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(SpiderProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProcessSpiderCommandResponse>> Process(
        [FromBody] ProcessSpiderCommandRequest request,
        ApiVersion apiVersion,
        CancellationToken ct)
    {
        Response.Headers.Append("api-version", apiVersion.ToString());

        _logger.LogInformation("Received spider command request");

        try
        {
            // TODO: maybe use fluent validation here
            if (string.IsNullOrWhiteSpace(request.WallInput) ||
                string.IsNullOrWhiteSpace(request.SpiderInput) ||
                string.IsNullOrWhiteSpace(request.CommandInput))
            {
                return BadRequest(new SpiderProblemDetails(
                    "Invalid Request",
                    "Required fields cannot be empty",
                    StatusCodes.Status400BadRequest));
            }

            var command = new ProcessSpiderCommand
            {
                WallInput = request.WallInput,
                SpiderInput = request.SpiderInput,
                CommandInput = request.CommandInput
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                // determine the type of error based on message content
                if (result.Error.Contains("invalid") || result.Error.Contains("format"))
                {
                    return BadRequest(new SpiderProblemDetails(
                        "Invalid Request",
                        result.Error,
                        StatusCodes.Status400BadRequest));
                }
                else if (result.Error.Contains("fall off") || result.Error.Contains("position") ||
                         result.Error.Contains("validation"))
                {
                    return UnprocessableEntity(new SpiderProblemDetails(
                        "Validation Failed",
                        result.Error,
                        StatusCodes.Status422UnprocessableEntity));
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new SpiderProblemDetails(
                            "Internal Server Error",
                            result.Error,
                            StatusCodes.Status500InternalServerError));
                }
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in SpiderController.Process");

            return StatusCode(StatusCodes.Status500InternalServerError,
                new SpiderProblemDetails(
                    "Internal Server Error",
                    "An unexpected error occurred",
                    StatusCodes.Status500InternalServerError));
        }
    }
}
