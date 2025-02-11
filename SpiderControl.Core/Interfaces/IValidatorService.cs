using FluentValidation.Results;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface IValidatorService
{
    ValidationResult ValidateWall(WallModel wall);
    ValidationResult ValidateSpider(SpiderModel spider);
    ValidationResult ValidateSpiderPosition(SpiderModel spider, WallModel wall);
    ValidationResult ValidateCommand(char command);
    ValidationResult ValidateCommands(IEnumerable<char> commands);
}
