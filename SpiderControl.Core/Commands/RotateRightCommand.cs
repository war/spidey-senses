using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Commands;

public class RotateRightCommand : ICommand
{
    public void Execute(Spider spider, WallModel wall)
    {
        spider.RotateRight();
    }
}
