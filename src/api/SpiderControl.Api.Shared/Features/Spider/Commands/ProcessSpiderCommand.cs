using MediatR;
using Microsoft.Extensions.Logging;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Models;
using SpiderControl.Core.Common;

namespace SpiderControl.Api.Shared.Features.Spider.Commands;

public record ProcessSpiderCommand : IRequest<Result<ProcessSpiderCommandResponse>>
{
    public required string WallInput { get; init; }
    public required string SpiderInput { get; init; }
    public required string CommandInput { get; init; }
}

public class ProcessSpiderCommandHandler : IRequestHandler<ProcessSpiderCommand, Result<ProcessSpiderCommandResponse>>
{
    private readonly ISpiderApplicationService _spiderService;
    private readonly ILogger<ProcessSpiderCommandHandler> _logger;

    public ProcessSpiderCommandHandler(
        ISpiderApplicationService spiderService,
        ILogger<ProcessSpiderCommandHandler> logger)
    {
        _spiderService = spiderService;
        _logger = logger;
    }

    public async Task<Result<ProcessSpiderCommandResponse>> Handle(
        ProcessSpiderCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing spider command with wall: {Wall}, spider: {Spider}, commands: {Commands}",
            command.WallInput, command.SpiderInput, command.CommandInput);

        var result = _spiderService.ProcessSpiderCommands(new ProcessCommandModel
        {
            WallInput = command.WallInput,
            SpiderInput = command.SpiderInput,
            CommandInput = command.CommandInput
        });

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Command processing failed: {Error}", result.Error);
            return Result<ProcessSpiderCommandResponse>.Failure(result.Error);
        }

        var response = new ProcessSpiderCommandResponse
        {
            FinalPosition = result.Value.ToString()
        };

        return Result<ProcessSpiderCommandResponse>.Success(response);
    }
}
