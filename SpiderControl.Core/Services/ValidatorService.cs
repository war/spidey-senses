using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpiderControl.Core.Configuration;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;
using SpiderControl.Core.Validators;

namespace SpiderControl.Core.Services;

public class ValidatorService : IValidatorService
{
    private readonly ILogger<ValidatorService> _logger;
    private readonly CommandValidator _commandValidator;

    public ValidatorService(ILogger<ValidatorService> logger, IOptions<SpiderControlConfig> config)
    {
        if (logger == null)
            throw new ArgumentNullException(nameof(logger));

        if (config == null)
            throw new ArgumentNullException(nameof(config));

        _logger = logger;
        _commandValidator = new CommandValidator(config);
    }

    public ValidationResult ValidateSpider(Spider spider)
    {
        _logger.LogDebug("Validating basic spider properties");
        var validator = new SpiderModelValidator();
        return validator.Validate(spider);
    }

    public ValidationResult ValidateSpiderPosition(Spider spider, WallModel wall)
    {
        _logger.LogDebug("Validating spider position");
        var validator = new SpiderPositionValidator(wall);
        return validator.Validate(spider);
    }

    public ValidationResult ValidateWall(WallModel wall)
    {
        _logger.LogDebug("Validating wall dimensions");
        var validator = new WallModelValidator();
        return validator.Validate(wall);
    }

    public ValidationResult ValidateCommand(char command)
    {
        _logger.LogDebug("Validating single command");
        return _commandValidator.Validate(command);
    }

    public ValidationResult ValidateCommands(IEnumerable<char> commands)
    {
        _logger.LogDebug("Validating command sequence");
        
        var results = commands.Select(c => _commandValidator.Validate(c));
        var failures = results.SelectMany(c => c.Errors).ToList();

        return new ValidationResult(failures);
    }
}
