using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Services;

public class InputParser : IInputParser
{
    public IEnumerable<ICommand> ParseCommands(string input)
    {
        return new List<ICommand>();
    }

    public SpiderModel ParseSpiderPosition(string input)
    {
        return new SpiderModel(1, 1, Enums.Orientation.Up);
    }

    public WallModel ParseWallDimensions(string input)
    {
        return new WallModel(1, 1);
    }
}
