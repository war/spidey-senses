using Microsoft.Extensions.Logging;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Models;
using SpiderControl.Core.Common;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

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

        var spiderResult = Result<Spider>.Failure("null");
        var wallResult = Result<WallModel>.Failure("null");
        var commandsResult = Result<IEnumerable<ICommand>>.Failure("null");

        var parseInputs = ParseInputs(model, ref spiderResult, ref wallResult, ref commandsResult);

        if (!parseInputs.IsSuccess)
            return Result<string>.Failure(parseInputs.Error);

        var processResult = _spiderService.ProcessCommands(
            spiderResult.Value,
            wallResult.Value,
            commandsResult.Value);

        return processResult.IsSuccess
            ? Result<string>.Success(processResult.Value.ToString())
            : Result<string>.Failure(processResult.Error);
    }

    public Result<List<string>> ProcessSpiderCommandsWithHistory(ProcessCommandModel model)
    {
        return Result<List<string>>.Failure("init");
    }

    private Result<string> ParseInputs(
        ProcessCommandModel model,
        ref Result<Spider> spiderResult,
        ref Result<WallModel> wallResult,
        ref Result<IEnumerable<ICommand>> commandsResult)
    {
        spiderResult = _spiderInputParser.ParseSpiderPosition(model.SpiderInput);
        if (!spiderResult.IsSuccess)
        {
            _logger.LogWarning("Failed to parse spider position: {Error}", spiderResult.Error);
            return Result<string>.Failure(spiderResult.Error);
        }

        wallResult = _wallInputParser.ParseWallDimensions(model.WallInput);
        if (!wallResult.IsSuccess)
        {
            _logger.LogWarning("Failed to parse wall dimensions: {Error}", wallResult.Error);
            return Result<string>.Failure(wallResult.Error);
        }

        commandsResult = _commandInputParser.ParseCommands(model.CommandInput);
        if (!commandsResult.IsSuccess)
        {
            _logger.LogWarning("Failed to parse commands: {Error}", commandsResult.Error);
            return Result<string>.Failure(commandsResult.Error);
        }

        return Result<string>.Success("success");
    }
}