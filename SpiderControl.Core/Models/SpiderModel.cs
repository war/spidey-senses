using SpiderControl.Core.Enums;

namespace SpiderControl.Core.Models;

public class SpiderModel
{
    public int X { get; set; }
    public int Y { get; set; }
    public Orientation Orientation { get; set; }

    public SpiderModel(int x, int y, Orientation orientation)
    {
        X = x; 
        Y = y;
        Orientation = orientation;
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
