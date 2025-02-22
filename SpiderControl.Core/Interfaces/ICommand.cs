using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface ICommand
{
    void Execute(Spider spider, WallModel wall);
}
