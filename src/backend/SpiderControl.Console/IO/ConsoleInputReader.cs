using Microsoft.Extensions.Logging;
using SpiderControl.Console.Interfaces;
using SpiderControl.Console.Models;
using SysConsole = System.Console;

namespace SpiderControl.Console.IO;

public class ConsoleInputReader : IConsoleInputReader
{
    private readonly ILogger<IConsoleInputReader> _logger;

    private const string WALL_DIMENSIONS_PROMPT = "Enter wall dimensions (e.g. '2 7'):";
    private const string SPIDER_POSITION_PROMPT = "Enter spider position & orientation (e.g. '1 5 Right'):";
    private const string COMMANDS_PROMPT = "Enter commands (e.g. 'FFRFLLF'):";

    public ConsoleInputReader(ILogger<IConsoleInputReader> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public InputModel ReadInputs()
    {
        try
        {
            var wallDimensions = ReadInput(WALL_DIMENSIONS_PROMPT, "wall dimensions");
            var spiderPosition = ReadInput(SPIDER_POSITION_PROMPT, "spider position");
            var commands = ReadInput(COMMANDS_PROMPT, "commands");

            _logger.LogInformation("Successfully read all inputs");

            return new InputModel(
                wallDimensions: wallDimensions,
                spiderPosition: spiderPosition,
                commands: commands);
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            _logger.LogError(ex, "Unexpected error while reading console inputs");
            throw new InvalidOperationException("Failed to read console inputs", ex);
        }
    }

    private string ReadInput(string prompt, string inputName)
    {
        System.Console.WriteLine(prompt);

        var input = System.Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(input))
        {
            var error = $"No input provided for {inputName}";
            _logger.LogError(error);
            throw new InvalidOperationException(error);
        }

        _logger.LogDebug("Read {InputName}: {Input}", inputName, input);
        return input;
    }
}
