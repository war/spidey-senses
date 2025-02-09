namespace SpiderControl.Application.Interfaces;

public interface ISpiderApplicationService
{
    public string ProcessSpiderCommands(string wallInput, string spiderInput, string commandInput);
}
