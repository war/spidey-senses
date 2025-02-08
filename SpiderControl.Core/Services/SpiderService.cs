using SpiderControl.Core.Models;
using SpiderControl.Core.Interfaces;

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

    public void MoveForward(SpiderModel model)
    {
        switch (model.Orientation)
        {
            case Orientation.Up:
                model.Y += 1;
                break;
            case Orientation.Right:
                model.X += 1;
                break;
            case Orientation.Down:
                model.Y -= 1;
                break;
            case Orientation.Left:
                model.X -= 1;
                break;
        }
    }

    public void RotateLeft(SpiderModel model)
    {
        switch (model.Orientation)
        {
            case Orientation.Up:
                model.Orientation = Orientation.Left; 
                break;
            case Orientation.Left:
                model.Orientation = Orientation.Down;
                break;
            case Orientation.Down:
                model.Orientation = Orientation.Right;
                break;
            case Orientation.Right:
                model.Orientation = Orientation.Up;
                break;
            default:
                throw new ArgumentException("Invalid orientation");
        }
    }

    public void RotateRight(SpiderModel model)
    {
        switch (model.Orientation)
        {
            case Orientation.Up:
                model.Orientation = Orientation.Right;
                break;
            case Orientation.Right:
                model.Orientation = Orientation.Down;
                break;
            case Orientation.Down:
                model.Orientation = Orientation.Left;
                break;
            case Orientation.Left:
                model.Orientation = Orientation.Up;
                break;
            default:
                throw new ArgumentException("Invalid orientation");
        }
    }
}
