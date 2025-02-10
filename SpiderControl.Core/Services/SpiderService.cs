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

    public SpiderModel CreateSpider(int x, int y, Orientation orientation)
    {
        return new SpiderModel(x, y, orientation);
    }

    public (int nextX, int nextY) GetNextForwardPosition(SpiderModel spider)
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

    public Orientation GetRightOrientation(Orientation orientation)
    {
        return orientation switch
        {
            Orientation.Up => Orientation.Right,
            Orientation.Right => Orientation.Down,
            Orientation.Down => Orientation.Left,
            Orientation.Left => Orientation.Up,
            _ => throw new ArgumentException("Invalid orientation")
        };
    }

    public Orientation GetLeftOrientation(Orientation orientation)
    {
        return orientation switch
        {
            Orientation.Up => Orientation.Left,
            Orientation.Left => Orientation.Down,
            Orientation.Down => Orientation.Right,
            Orientation.Right => Orientation.Up,
            _ => throw new ArgumentException("Invalid orientation")
        };
    }

    public bool IsValidMove(SpiderModel spider, WallModel wall, int nextX, int nextY)
    {
        return nextX >= 0 && nextY >= 0 && nextX <= wall.Width && nextY <= wall.Height;
    }

    public void MoveForward(SpiderModel spider)
    {
        (spider.X, spider.Y) = GetNextForwardPosition(spider);
    }

    public void RotateLeft(SpiderModel spider)
    {
        var orientation = GetLeftOrientation(spider.Orientation);
        spider.Orientation = orientation;
    }

    public void RotateRight(SpiderModel spider)
    {
        var orientation = GetRightOrientation(spider.Orientation);
        spider.Orientation = orientation;
    }

    public SpiderModel ProcessCommands(SpiderModel spider, WallModel wall, IEnumerable<ICommand> commands)
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
