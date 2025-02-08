namespace SpiderControl.Core.Interfaces;

public interface ICommandFactory
{
    ICommand CreateCommand(char commandChar);
}
