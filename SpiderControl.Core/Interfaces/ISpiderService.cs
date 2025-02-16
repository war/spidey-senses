using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface ISpiderService
{
    Spider ProcessCommands(Spider spider, WallModel wall, IEnumerable<ICommand> commands);
}
