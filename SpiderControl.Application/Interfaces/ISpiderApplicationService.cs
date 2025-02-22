using SpiderControl.Application.Models;

namespace SpiderControl.Application.Interfaces;

public interface ISpiderApplicationService
{
    string ProcessSpiderCommands(ProcessCommandModel model);
}
