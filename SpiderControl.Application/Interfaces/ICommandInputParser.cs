using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Interfaces;

public interface ICommandInputParser
{
    IEnumerable<ICommand> ParseCommands(string input);
}
