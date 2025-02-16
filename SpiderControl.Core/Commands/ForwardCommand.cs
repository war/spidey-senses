using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Commands;

public class ForwardCommand : ICommand
{
    public void Execute(Spider spider, WallModel wall, ISpiderService spiderService)
    {
        if (!Validate(spider, wall, spiderService))
        {
            throw new InvalidOperationException("Invalid move: Spider would fall off the wall :(.");
        }

        spiderService.MoveForward(spider);
    }

    public bool Validate(Spider spider, WallModel wall, ISpiderService spiderService)
    {
        var getNextMove = spiderService.GetNextForwardPosition(spider);
        return spiderService.IsValidMove(spider, wall, getNextMove.nextX, getNextMove.nextY);
    }
}
