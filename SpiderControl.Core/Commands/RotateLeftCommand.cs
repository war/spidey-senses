using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Commands;

public class RotateLeftCommand : ICommand
{
    public void Execute(SpiderModel spider, WallModel wall, ISpiderService spiderService)
    {
        spiderService.RotateLeft(spider);
    }
}
