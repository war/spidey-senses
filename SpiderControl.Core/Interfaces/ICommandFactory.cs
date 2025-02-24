using SpiderControl.Core.Common;

namespace SpiderControl.Core.Interfaces;

public interface ICommandFactory
{
    Result<ICommand> CreateCommand(char commandChar);
}
