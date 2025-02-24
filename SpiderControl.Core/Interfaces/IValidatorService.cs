using SpiderControl.Core.Common;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface IValidatorService
{
    Result<Unit> ValidateWall(WallModel wall);
    Result<Unit> ValidateSpider(Spider spider);
    Result<Unit> ValidateSpiderPosition(Spider spider, WallModel wall);
    Result<Unit> ValidateCommand(char command);
    Result<Unit> ValidateCommands(IEnumerable<char> commands);
}
