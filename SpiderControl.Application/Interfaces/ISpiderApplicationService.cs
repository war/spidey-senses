using SpiderControl.Application.Models;

namespace SpiderControl.Application.Interfaces;

public interface ISpiderApplicationService
{
    public string ProcessSpiderCommands(ProcessCommandModel model);
}
