using SpiderControl.Core.Enums;

namespace SpiderControl.Core.Models;

public class Spider
{
    public int X { get; set; }
    public int Y { get; set; }
    public Orientation Orientation { get; set; }

    public Spider(int x, int y, Orientation orientation)
    {
        X = x; 
        Y = y;
        Orientation = orientation;
    }

    public void MoveForward()
    {
        var newSpider = GetNextForwardPosition();

        this.X = newSpider.X;
        this.Y = newSpider.Y;
    }

    public void RotateLeft()
    {
        var orientation = this.GetLeftOrientation();
        this.Orientation = orientation;
    }

    public void RotateRight()
    {
        var orientation = this.GetRightOrientation();
        this.Orientation = orientation;
    }

    public Spider GetNextForwardPosition()
    {
        var nextX = this.X;
        var nextY = this.Y;

        switch (this.Orientation)
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

        return new Spider(nextX, nextY, this.Orientation);
    }

    public Orientation GetRightOrientation()
    {
        return this.Orientation switch
        {
            Orientation.Up => Orientation.Right,
            Orientation.Right => Orientation.Down,
            Orientation.Down => Orientation.Left,
            Orientation.Left => Orientation.Up,
            _ => throw new ArgumentException("Invalid orientation")
        };
    }

    public Orientation GetLeftOrientation()
    {
        return this.Orientation switch
        {
            Orientation.Up => Orientation.Left,
            Orientation.Left => Orientation.Down,
            Orientation.Down => Orientation.Right,
            Orientation.Right => Orientation.Up,
            _ => throw new ArgumentException("Invalid orientation")
        };
    }

    public override string ToString()
    {
        var orientation = this.Orientation switch
        {
            Orientation.Up => "Up",
            Orientation.Right => "Right",
            Orientation.Down => "Down",
            Orientation.Left => "Left",
            _ => throw new ArgumentException($"Invalid orientation: {this.Orientation}")
        };

        return $"{this.X} {this.Y} {orientation}";
    }
}
