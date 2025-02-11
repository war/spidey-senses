using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Services;

public class ValidatorService : IValidatorService
{
    private readonly ILogger<ValidatorService> _logger;

    public ValidatorService(ILogger<ValidatorService> logger)
    {
        _logger = logger;
    }

    public ValidationResult ValidateSpider(SpiderModel spider, WallModel wall)
    {
        throw new NotImplementedException();
    }

    public ValidationResult ValidateWall(WallModel wall)
    {
        throw new NotImplementedException();
    }

    public ValidationResult ValidateCommand(char command)
    {
        throw new NotImplementedException();
    }

    public ValidationResult ValidateCommands(IEnumerable<char> comamnds)
    {
        throw new NotImplementedException();
    }
}
