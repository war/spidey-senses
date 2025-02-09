using SpiderControl.Console.Interfaces;
using SpiderControl.Console.Models;

namespace SpiderControl.Console.IO;

public class ConsoleInputReader : IConsoleInputReader
{
    public InputModel ReadInputs()
    {
        return new InputModel();
    }
}
