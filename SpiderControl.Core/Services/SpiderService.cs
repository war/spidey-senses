using SpiderControl.Core.Models;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Enums;

namespace SpiderControl.Core.Services;

public class SpiderService : ISpiderService
{
    public SpiderService()
    {
        
    }

    public SpiderModel CreateSpider(int x, int y, Orientation orientation)
    {
        return new SpiderModel(x, y, orientation);
    }

    public (int nextX, int nextY) GetNextForwardPosition(SpiderModel spider)
    {
        var nextX = spider.X;
        var nextY = spider.Y;

        switch (spider.Orientation)
        {
            case Orientation.Up:
                nextY += 1;
                break;
            case Orientation.Right:
                nextX += 1;
                break;
            case Orientation.Down:
                nextY -= 1;
                break;
            case Orientation.Left:
                nextX -= 1;
                break;
        }

        return (nextX, nextY);
    }

    public bool IsValidMove(SpiderModel spider, WallModel wall, int nextX, int nextY)
    {
        return nextX >= 0 && nextY >= 0 && nextX <= wall.Width && nextY <= wall.Height;
    }

    public void MoveForward(SpiderModel spider)
    {
        var getNextForward = GetNextForwardPosition(spider);
        spider.X = getNextForward.nextX;
        spider.Y = getNextForward.nextY;
    }

    public void RotateLeft(SpiderModel spider)
    {
        switch (spider.Orientation)
        {
            case Orientation.Up:
                spider.Orientation = Orientation.Left; 
                break;
            case Orientation.Left:
                spider.Orientation = Orientation.Down;
                break;
            case Orientation.Down:
                spider.Orientation = Orientation.Right;
                break;
            case Orientation.Right:
                spider.Orientation = Orientation.Up;
                break;
            default:
                throw new ArgumentException("Invalid orientation");
        }
    }

    public void RotateRight(SpiderModel spider)
    {
        switch (spider.Orientation)
        {
            case Orientation.Up:
                spider.Orientation = Orientation.Right;
                break;
            case Orientation.Right:
                spider.Orientation = Orientation.Down;
                break;
            case Orientation.Down:
                spider.Orientation = Orientation.Left;
                break;
            case Orientation.Left:
                spider.Orientation = Orientation.Up;
                break;
            default:
                throw new ArgumentException("Invalid orientation");
        }
    }
}
