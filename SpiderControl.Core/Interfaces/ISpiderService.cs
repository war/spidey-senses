using SpiderControl.Core.Enums;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface ISpiderService
{
    Spider CreateSpider(int x, int y, Orientation orientation);

    bool IsValidMove(Spider spider, WallModel wall, int nextX, int nextY);

    Spider ProcessCommands(Spider spider, WallModel wall, IEnumerable<ICommand> commands);
}
