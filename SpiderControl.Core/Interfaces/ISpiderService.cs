using SpiderControl.Core.Enums;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Interfaces;

public interface ISpiderService
{
    SpiderModel CreateSpider(int x, int y, Orientation orientation);

    void RotateLeft(SpiderModel spider);
    void RotateRight(SpiderModel spider);
    void MoveForward(SpiderModel spider);

    (int nextX, int nextY) GetNextForwardPosition(SpiderModel spider);
    bool IsValidMove(SpiderModel spider, WallModel wall, int nextX, int nextY);

    Orientation GetLeftOrientation(Orientation orientation);
    Orientation GetRightOrientation(Orientation orientation);

    SpiderModel ProcessCommands(SpiderModel spider, WallModel wall, IEnumerable<ICommand> commands);
}
