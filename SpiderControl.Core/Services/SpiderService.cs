using SpiderControl.Core.Models;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Enums;
using Microsoft.Extensions.Logging;

namespace SpiderControl.Core.Services;

public class SpiderService : ISpiderService
{
    private readonly ILogger _logger;

    public SpiderService(ILogger<SpiderService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Spider CreateSpider(int x, int y, Orientation orientation)
    {
        return new Spider(x, y, orientation);
    }

    public (int nextX, int nextY) GetNextForwardPosition(Spider spider)
    {
        var nextX = spider.X;
        var nextY = spider.Y;

        switch (spider.Orientation)
        {
            case Orientation.Up:
                nextY += 1;
                break;
            case Orientation.Right:
                nextX += 1;
                break;
            case Orientation.Down:
                nextY -= 1;
                break;
            case Orientation.Left:
                nextX -= 1;
                break;
        }

        return (nextX, nextY);
    }

    public bool IsValidMove(Spider spider, WallModel wall, int nextX, int nextY)
    {
        return nextX >= 0 && nextY >= 0 && nextX <= wall.Width && nextY <= wall.Height;
    }

    public void MoveForward(Spider spider)
    {
        (spider.X, spider.Y) = GetNextForwardPosition(spider);
    }

    public void RotateLeft(Spider spider)
    {
        var orientation = spider.GetLeftOrientation();
        spider.Orientation = orientation;
    }

    public void RotateRight(Spider spider)
    {
        var orientation = spider.GetRightOrientation();
        spider.Orientation = orientation;
    }

    public Spider ProcessCommands(Spider spider, WallModel wall, IEnumerable<ICommand> commands)
    {
        foreach (var command in commands)
        {
            try
            {
                command.Execute(spider, wall, this);

                _logger.LogInformation("Executed command {Command}. New position: (x:{X}, y:{Y}) facing {Orientation})",
                    command.GetType().Name, spider.X, spider.Y, spider.Orientation);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid move.");
            }
        }

        _logger.LogInformation("Command processing complete");

        return spider;
    }
}
