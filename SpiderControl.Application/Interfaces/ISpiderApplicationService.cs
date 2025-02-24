using SpiderControl.Application.Models;
using SpiderControl.Core.Common;

namespace SpiderControl.Application.Interfaces;

public interface ISpiderApplicationService
{
    Result<string> ProcessSpiderCommands(ProcessCommandModel model);
}
