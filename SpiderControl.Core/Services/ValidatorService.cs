﻿using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;
using SpiderControl.Core.Validators;

namespace SpiderControl.Core.Services;

public class ValidatorService : IValidatorService
{
    private readonly ILogger<ValidatorService> _logger;

    public ValidatorService(ILogger<ValidatorService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        var validator = new CommandValidator();
        return validator.Validate(command);
    }

    public ValidationResult ValidateCommands(IEnumerable<char> commands)
    {
        _logger.LogDebug("Validating command sequence");
        
        var validator = new CommandValidator();
        var results = commands.Select(c => validator.Validate(c));
        var failures = results.SelectMany(c => c.Errors).ToList();

        return new ValidationResult(failures);
    }
}
