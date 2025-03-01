using SpiderControl.Core.Common;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Commands;

public class ForwardCommand : ICommand
{
    public Result<Unit> Execute(Spider spider, WallModel wall)
    {
        var nextPosition = spider.GetNextForwardPosition();

        if (!nextPosition.IsSuccess) 
            return Result<Unit>.Failure(nextPosition.Error);

        if (!IsValidMove(nextPosition.Value, wall))
            return Result<Unit>.Failure("Spider would fall off the wall");

        return spider.MoveForward();
    }

    private bool IsValidMove(Spider nextPosition, WallModel wall) =>
        nextPosition.X >= 0 && nextPosition.Y >= 0 &&
        nextPosition.X <= wall.Width && nextPosition.Y <= wall.Height;
}
