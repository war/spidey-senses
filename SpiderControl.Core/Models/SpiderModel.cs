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
