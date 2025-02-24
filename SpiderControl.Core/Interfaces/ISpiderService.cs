using SpiderControl.Core.Common;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface ISpiderService
{
    Result<Spider> ProcessCommands(Spider spider, WallModel wall, IEnumerable<ICommand> commands);
}
