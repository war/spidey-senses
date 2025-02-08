using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface ICommand
{
    public void Execute(SpiderModel spider, WallModel wall, ISpiderService spiderService);
}
