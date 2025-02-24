using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpiderControl.Core.Common;
using SpiderControl.Core.Configuration;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;
using SpiderControl.Core.Validators;

namespace SpiderControl.Core.Services;

public class ValidatorService : IValidatorService
{
    private readonly ILogger<ValidatorService> _logger;
    private readonly CommandValidator _commandValidator;
    private readonly SpiderModelValidator _spiderValidator;
    private readonly WallModelValidator _wallValidator;

    public ValidatorService(ILogger<ValidatorService> logger, IOptions<SpiderControlConfig> config)
    {
        if (logger == null)
            throw new ArgumentNullException(nameof(logger));

        if (config == null)
            throw new ArgumentNullException(nameof(config));

        _logger = logger;

        _spiderValidator = new SpiderModelValidator();
        _wallValidator = new WallModelValidator();
        _commandValidator = new CommandValidator(config);
    }

    public Result<Unit> ValidateSpider(Spider spider)
    {
        _logger.LogDebug("Validating basic spider properties");
        var result = _spiderValidator.Validate(spider);

        return result.IsValid
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(FormatValidationErrors(result));
    }

    public Result<Unit> ValidateSpiderPosition(Spider spider, WallModel wall)
    {
        _logger.LogDebug("Validating spider position");
        var validator = new SpiderPositionValidator(wall);
        var result = validator.Validate(spider);

        return result.IsValid
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(FormatValidationErrors(result));
    }

    public Result<Unit> ValidateWall(WallModel wall)
    {
        _logger.LogDebug("Validating wall dimensions");
        var result = _wallValidator.Validate(wall);

        return result.IsValid
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(FormatValidationErrors(result));
    }

    public Result<Unit> ValidateCommand(char command)
    {
        _logger.LogDebug("Validating single command");
        var result = _commandValidator.Validate(command);

        return result.IsValid
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(FormatValidationErrors(result));
    }

    public Result<Unit> ValidateCommands(IEnumerable<char> commands)
    {
        _logger.LogDebug("Validating command sequence");
        var failures = new List<ValidationFailure>();

        foreach (var command in commands)
        {
            var result = _commandValidator.Validate(command);
            if (!result.IsValid)
            {
                failures.AddRange(result.Errors);
            }
        }

        return failures.Count == 0
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(FormatValidationErrors(new ValidationResult(failures)));
    }

    private static string FormatValidationErrors(ValidationResult validationResult)
    {
        return string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
    }
}
