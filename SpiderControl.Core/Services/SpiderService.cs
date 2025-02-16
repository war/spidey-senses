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

    public bool IsValidMove(Spider spider, WallModel wall, int nextX, int nextY)
    {
        return nextX >= 0 && nextY >= 0 && nextX <= wall.Width && nextY <= wall.Height;
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
