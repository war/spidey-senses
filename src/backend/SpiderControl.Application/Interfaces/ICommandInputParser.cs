using SpiderControl.Core.Common;
using SpiderControl.Core.Interfaces;

namespace SpiderControl.Application.Interfaces;

public interface ICommandInputParser
{
    Result<IEnumerable<ICommand>> ParseCommands(string input);
}
