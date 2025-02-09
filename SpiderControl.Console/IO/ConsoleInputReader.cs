using SpiderControl.Console.Interfaces;
using SpiderControl.Console.Models;

namespace SpiderControl.Console.IO;

public class ConsoleInputReader : IConsoleInputReader
{
    public InputModel ReadInputs()
    {
        System.Console.WriteLine("Enter wall dimensions (e.g. '2 7'):");
        var wallDimensions = System.Console.ReadLine();

        if (string.IsNullOrEmpty(wallDimensions))
        {
            throw new InvalidOperationException("No input provided for wall dimensions");
        }

        System.Console.WriteLine("Enter spider position & orientation (e.g. '1 5 Right'):");
        var spiderPosition = System.Console.ReadLine();

        if (string.IsNullOrEmpty(spiderPosition))
        {
            throw new InvalidOperationException("No input provided for spider position");
        }

        System.Console.WriteLine("Enter commands (e.g. 'FFRFLLF'):");
        var commands = System.Console.ReadLine();

        if (string.IsNullOrEmpty(commands))
        {
            throw new InvalidOperationException("No input provided for commands");
        }

        return new InputModel(wallDimensions, spiderPosition, commands);
    }
}
