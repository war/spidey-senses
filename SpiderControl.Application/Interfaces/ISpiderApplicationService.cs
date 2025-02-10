namespace SpiderControl.Application.Interfaces;

public interface ISpiderApplicationService
{
    public string ProcessSpiderCommands(string spiderInput, string wallInput, string commandInput);
}
