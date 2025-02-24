using Microsoft.Extensions.Logging;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Models;
using SpiderControl.Core.Common;
using SpiderControl.Core.Interfaces;

namespace SpiderControl.Application.Services;

public class SpiderApplicationService : ISpiderApplicationService
{
    private readonly ISpiderService _spiderService;

    private readonly IWallInputParser _wallInputParser;
    private readonly ISpiderInputParser _spiderInputParser;
    private readonly ICommandInputParser _commandInputParser;
    private readonly ILogger<ISpiderApplicationService> _logger;

    public SpiderApplicationService(
        ISpiderService spiderService,
        IWallInputParser wallInputParser,
        ISpiderInputParser spiderInputParser,
        ICommandInputParser commandInputParser,
        ILogger<ISpiderApplicationService> logger)
    {
        _spiderService = spiderService ?? throw new ArgumentNullException(nameof(spiderService));
        _wallInputParser = wallInputParser ?? throw new ArgumentNullException(nameof(wallInputParser));
        _spiderInputParser = spiderInputParser ?? throw new ArgumentNullException(nameof(spiderInputParser));
        _commandInputParser = commandInputParser ?? throw new ArgumentNullException(nameof(commandInputParser));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Result<string> ProcessSpiderCommands(ProcessCommandModel model)
    {
        _logger.LogInformation("Processing commands for spider");

        var wallResult = _wallInputParser.ParseWallDimensions(model.WallInput);
        if (!wallResult.IsSuccess)
        {
            _logger.LogWarning("Failed to parse wall dimensions: {Error}", wallResult.Error);
            return Result<string>.Failure($"Invalid wall dimensions: {wallResult.Error}");
        }

        var spiderResult = _spiderInputParser.ParseSpiderPosition(model.SpiderInput);
        if (!spiderResult.IsSuccess)
        {
            _logger.LogWarning("Failed to parse spider position: {Error}", spiderResult.Error);
            return Result<string>.Failure($"Invalid spider position: {spiderResult.Error}");
        }

        var commandsResult = _commandInputParser.ParseCommands(model.CommandInput);
        if (!commandsResult.IsSuccess)
        {
            _logger.LogWarning("Failed to parse commands: {Error}", commandsResult.Error);
            return Result<string>.Failure($"Invalid commands: {commandsResult.Error}");
        }

        var validatorService = _spiderService as IValidatorService;
        if (validatorService != null)
        {
            var positionValidation = validatorService.ValidateSpiderPosition(
                spiderResult.Value, wallResult.Value);

            if (!positionValidation.IsSuccess)
            {
                _logger.LogWarning("Invalid spider position relative to wall: {Error}",
                    positionValidation.Error);
                return Result<string>.Failure(
                    $"Spider position outside wall boundaries: {positionValidation.Error}");
            }
        }
        
        var processResult = _spiderService.ProcessCommands(
            spiderResult.Value, wallResult.Value, commandsResult.Value);

        if (!processResult.IsSuccess)
        {
            _logger.LogWarning("Command processing failed: {Error}", processResult.Error);
            return Result<string>.Failure($"Failed to process commands: {processResult.Error}");
        }

        _logger.LogInformation("Successfully processed commands, final position: {Position}",
            processResult.Value.ToString());

        return Result<string>.Success(processResult.Value.ToString());
    }
}