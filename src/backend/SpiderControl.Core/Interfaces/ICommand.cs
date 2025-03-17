using SpiderControl.Core.Common;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface ICommand
{
    Result<Unit> Execute(Spider spider, WallModel wall);
}
