using Microsoft.Extensions.Logging;
using SpiderControl.Application.Interfaces;
using SpiderControl.Core.Common;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Services;

public class SpiderInputParser : ISpiderInputParser
{
    private readonly IValidatorService _validatorService;
    private readonly ILogger<ISpiderInputParser> _logger;

    public SpiderInputParser(IValidatorService validatorService, ILogger<ISpiderInputParser> logger)
    {
        _validatorService = validatorService;
        _logger = logger;
    }

    public Result<Spider> ParseSpiderPosition(string input)
    {
        _logger.LogDebug("Parsing spider position: {Input}", input);

        if (string.IsNullOrWhiteSpace(input))
        {
            _logger.LogWarning("Spider position input is empty");
            return Result<Spider>.Failure("Spider position cannot be empty");
        }

        _logger.LogDebug("Parsing spider position from: {Input}", input);

        var args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (args.Length != 3)
        {
            _logger.LogWarning("Invalid format for spider position: {Input}", input);
            return Result<Spider>.Failure("Invalid format. Expected: 'x y orientation'");
        }

        if (!int.TryParse(args[0], out int x))
        {
            _logger.LogWarning("Invalid X coordinate: {X}", args[0]);
            return Result<Spider>.Failure("Invalid X coordinate. Must be a valid integer");
        }

        if (!int.TryParse(args[1], out int y))
        {
            _logger.LogWarning("Invalid Y coordinate: {Y}", args[1]);
            return Result<Spider>.Failure("Invalid Y coordinate. Must be a valid integer");
        }

        if (!Enum.TryParse<Orientation>(args[2], true, out var orientation) && Enum.IsDefined(typeof(Orientation), orientation))
        {
            _logger.LogWarning("Invalid orientation: {Orientation}", args[2]);
            return Result<Spider>.Failure(
                $"Invalid orientation '{args[2]}'. Must be one of: {string.Join(", ", Enum.GetNames<Orientation>())}");
        }

        var spider = new Spider(x, y, orientation);
        var validationResult = _validatorService.ValidateSpider(spider);

        if (!validationResult.IsSuccess)
        {
            _logger.LogWarning("Spider validation failed: {Error}", validationResult.Error);
            return Result<Spider>.Failure(validationResult.Error);
        }

        _logger.LogDebug("Successfully parsed spider position: X={X}, Y={Y}, Orientation={Orientation}",
            x, y, orientation);

        return Result<Spider>.Success(spider);
    }
}
