using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface ISpiderService
{
    SpiderModel CreateSpider(int x, int y, Orientation orientation);
}
