using Microsoft.Extensions.Logging;
using SpiderControl.Application.Interfaces;
using SpiderControl.Core.Common;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Services;

public class WallInputParser : IWallInputParser
{
    private readonly IValidatorService _validatorService;
    private readonly ILogger<IWallInputParser> _logger;

    public WallInputParser(IValidatorService validatorService, ILogger<IWallInputParser> logger)
    {
        _validatorService = validatorService;
        _logger = logger;
    }

    public Result<WallModel> ParseWallDimensions(string input)
    {
        _logger.LogDebug("Parsing wall dimensions: {Input}", input);

        var args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (args.Length != 2)
        {
            _logger.LogWarning("Invalid wall dimension format: {Input}", input);
            return Result<WallModel>.Failure("Invalid wall dimensions format. Expected: 'width height'");
        }

        if (!int.TryParse(args[0], out int width))
        {
            _logger.LogWarning("Invalid wall width: {Width}", args[0]);
            return Result<WallModel>.Failure("Invalid wall width. Expected an integer");
        }

        if (!int.TryParse(args[1], out int height))
        {
            _logger.LogWarning("Invalid wall height: {Height}", args[1]);
            return Result<WallModel>.Failure("Invalid wall height. Expected an integer");
        }

        var wall = new WallModel(width, height);
        var validationResult = _validatorService.ValidateWall(wall);

        if (!validationResult.IsSuccess)
        {
            _logger.LogWarning("Wall validation failed: {Error}", validationResult.Error);
            return Result<WallModel>.Failure(validationResult.Error);
        }

        _logger.LogDebug("Wall parsed successfully: {Width}x{Height}", width, height);
        return Result<WallModel>.Success(wall);
    }
}
