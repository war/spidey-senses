using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface ICommand
{
    public void Execute(Spider spider, WallModel wall, ISpiderService spiderService);
}
