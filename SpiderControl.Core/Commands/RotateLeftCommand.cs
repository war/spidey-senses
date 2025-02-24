using SpiderControl.Core.Common;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Commands;

public class RotateLeftCommand : ICommand
{
    public Result<Unit> Execute(Spider spider, WallModel wall)
    {
        return spider.RotateLeft();
    }
}
