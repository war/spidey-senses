using SpiderControl.Core.Commands;
using SpiderControl.Core.Common;
using SpiderControl.Core.Interfaces;

namespace SpiderControl.Core.Factories;

public class CommandFactory : ICommandFactory
{
    public Result<ICommand> CreateCommand(char commandChar)
    {
        return char.ToUpper(commandChar) switch
        {
            'F' => Result<ICommand>.Success(new ForwardCommand()),
            'R' => Result<ICommand>.Success(new RotateRightCommand()),
            'L' => Result<ICommand>.Success(new RotateLeftCommand()),
            _ => Result<ICommand>.Failure($"Invalid command character: {commandChar}")
        };
    }
}
