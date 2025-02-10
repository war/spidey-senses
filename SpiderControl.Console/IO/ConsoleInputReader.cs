using SpiderControl.Console.Interfaces;
using SpiderControl.Console.Models;
using SysConsole = System.Console;

namespace SpiderControl.Console.IO;

public class ConsoleInputReader : IConsoleInputReader
{
    public InputModel ReadInputs()
    {
        SysConsole.WriteLine("Enter wall dimensions (e.g. '2 7'):");
        var wallDimensions = SysConsole.ReadLine();

        if (string.IsNullOrEmpty(wallDimensions))
        {
            throw new InvalidOperationException("No input provided for wall dimensions");
        }

        SysConsole.WriteLine("Enter spider position & orientation (e.g. '1 5 Right'):");
        var spiderPosition = SysConsole.ReadLine();

        if (string.IsNullOrEmpty(spiderPosition))
        {
            throw new InvalidOperationException("No input provided for spider position");
        }

        SysConsole.WriteLine("Enter commands (e.g. 'FFRFLLF'):");
        var commands = SysConsole.ReadLine();

        if (string.IsNullOrEmpty(commands))
        {
            throw new InvalidOperationException("No input provided for commands");
        }

        return new InputModel(wallDimensions, spiderPosition, commands);
    }
}
