using SpiderControl.Core.Models;
using SpiderControl.Core.Interfaces;
using Microsoft.Extensions.Logging;
using SpiderControl.Core.Common;

namespace SpiderControl.Core.Services;

public class SpiderService : ISpiderService
{
    private readonly ILogger _logger;

    public SpiderService(ILogger<ISpiderService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Result<Spider> ProcessCommands(Spider spider, WallModel wall, IEnumerable<ICommand> commands)
    {
        var commandList = commands.ToList();

        for (int i = 0; i < commandList.Count; i++)
        {
            var result = commandList[i].Execute(spider, wall);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Command {Index} failed: {Error}", i + 1, result.Error);
                return Result<Spider>.Failure($"Command {i + 1} failed: {result.Error}");
            }

            _logger.LogInformation("Command {Index}/{Total} executed successfully", i + 1, commandList.Count);
        }

        return Result<Spider>.Success(spider);
    }
}
