using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Commands;

public class RotateLeftCommand : ICommand
{
    public void Execute(Spider spider, WallModel wall, ISpiderService spiderService)
    {
        spider.RotateLeft();
    }
}
