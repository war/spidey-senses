using SpiderControl.Core.Enums;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface ISpiderService
{
    SpiderModel CreateSpider(int x, int y, Orientation orientation);

    void RotateLeft(SpiderModel model);
    void RotateRight(SpiderModel model);
    void MoveForward(SpiderModel model);
}
