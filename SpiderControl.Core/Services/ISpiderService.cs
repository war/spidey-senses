using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface ISpiderService
{
    SpiderModel CreateSpider(int x, int y, Orientation orientation);

    void TurnLeft(SpiderModel model);
    void TurnRight(SpiderModel model);
    void MoveForward(SpiderModel model);
}
