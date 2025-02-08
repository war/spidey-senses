using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface IInputParser
{
    WallModel ParseWallDimensions(string input);
    SpiderModel ParseSpiderPosition(string input);
    IEnumerable<ICommand> ParseCommands(string input);
}
