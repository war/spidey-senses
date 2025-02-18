using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;
using static System.Net.Mime.MediaTypeNames;

namespace SpiderControl.Core.Commands;

public class ForwardCommand : ICommand
{
    public void Execute(Spider spider, WallModel wall)
    {
        if (!IsValidMove(spider, wall))
        {
            throw new InvalidOperationException("Invalid move: Spider would fall off the wall :(.");
        }

        spider.MoveForward();
    }

    public bool IsValidMove(Spider spider, WallModel wall)
    {
        var getNextMove = spider.GetNextForwardPosition();
        return getNextMove.X >= 0 && getNextMove.Y >= 0 && getNextMove.X <= wall.Width && getNextMove.Y <= wall.Height;
    }
}
